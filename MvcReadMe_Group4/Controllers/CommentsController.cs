using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using MvcReadMe_Group4.Data;
using MvcReadMe_Group4.Models;
using MvcReadMe_Group4.Hubs;
using System;
using System.Threading.Tasks;

namespace MvcReadMe_Group4.Controllers
{
    public class CommentsController : Controller
    {
        private readonly MvcReadMe_Group4Context _context;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly Microsoft.Extensions.Logging.ILogger<CommentsController> _logger;

        public CommentsController(MvcReadMe_Group4Context context, IHubContext<NotificationHub> hubContext, Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Logging.ILogger<CommentsController> logger)
        {
            _context = context;
            _hubContext = hubContext;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetForBook(int bookId)
        {
            try
            {
                var results = new List<object>();
                var seen = new HashSet<long>();

                // Try SQLite first (app DB)
                try
                {
                    var sqliteComments = await _context.Comments
                        .AsNoTracking()
                        .Include(c => c.User)
                        .Where(c => c.BookId == bookId && !c.IsHidden)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToListAsync();

                    foreach (var c in sqliteComments)
                    {
                        results.Add(new {
                            commentId = c.CommentId,
                            userId = c.UserId,
                            userName = c.User != null ? c.User.UserName : ("User " + c.UserId),
                            text = c.CommentText,
                            createdAt = c.CreatedAt
                        });
                        seen.Add(c.CommentId);
                    }
                }
                catch (Exception sx)
                {
                    // SQLite read failed — log and continue to try MySQL
                    _logger.LogWarning(sx, "Reading comments from SQLite failed; will attempt MySQL for book {BookId}", bookId);
                }

                // Also try MySQL (mirror) to include comments from php side
                try
                {
                    var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                    if (!string.IsNullOrEmpty(mySqlConn))
                    {
                        await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                        await mconn.OpenAsync();
                        var cmd = mconn.CreateCommand();
                        cmd.CommandText = "SELECT c.CommentId, c.UserId, c.CommentText, c.CreatedAt, u.UserName FROM comments c LEFT JOIN users u ON c.UserId = u.Id WHERE c.BookId = @b AND (c.IsHidden IS NULL OR c.IsHidden = 0) ORDER BY c.CreatedAt DESC";
                        cmd.Parameters.AddWithValue("@b", bookId);
                        await using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            var id = reader.IsDBNull(0) ? 0L : reader.GetInt64(0);
                            if (id != 0 && seen.Contains(id)) continue;
                            long userId = reader.IsDBNull(1) ? 0L : reader.GetInt64(1);
                            string text = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            string? created = reader.IsDBNull(3) ? null : reader.GetString(3);
                            string userName = reader.IsDBNull(4) ? ("User " + userId) : reader.GetString(4);

                            DateTime? createdAt = null;
                            if (!string.IsNullOrEmpty(created) && DateTime.TryParse(created, out var dt)) createdAt = dt;

                            results.Add(new {
                                commentId = id,
                                userId = (int)userId,
                                userName = userName,
                                text = text,
                                createdAt = createdAt
                            });
                            if (id != 0) seen.Add(id);
                        }
                    }
                }
                catch (Exception mx)
                {
                    _logger.LogWarning(mx, "Reading comments from MySQL failed for book {BookId}", bookId);
                }

                return Json(new { success = true, comments = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comments for book {BookId}", bookId);
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int bookId, string text)
        {
            try
            {
                int? userId = GetCurrentUserId();
                if (userId == null) return Json(new { success = false, error = "Not authenticated" });

                var comment = new Comment
                {
                    BookId = bookId,
                    UserId = userId.Value,
                    CommentText = text,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Comments.Add(comment);

                bool sqliteSaved = false;
                try
                {
                    await _context.SaveChangesAsync();
                    sqliteSaved = true;
                }
                catch (Exception saveEx)
                {
                    // If saving to SQLite fails (table missing, etc.) we will attempt MySQL mirror.
                    _logger.LogWarning(saveEx, "Saving comment to SQLite failed; will attempt MySQL mirror and continue.");
                }

                // Best-effort dual-write to MySQL so admin_report and phpMyAdmin see comments
                bool mysqlSaved = false;
                try
                {
                    var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                    if (!string.IsNullOrEmpty(mySqlConn))
                    {
                        await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                        await mconn.OpenAsync();
                        var ins = mconn.CreateCommand();
                        // Use PascalCase column names in MySQL (CommentId auto, UserId, BookId, CommentText, CreatedAt, IsHidden)
                        ins.CommandText = "INSERT INTO comments (UserId, BookId, CommentText, CreatedAt, IsHidden) VALUES (@u,@b,@t,@c,0)";
                        ins.Parameters.AddWithValue("@u", comment.UserId);
                        ins.Parameters.AddWithValue("@b", comment.BookId);
                        ins.Parameters.AddWithValue("@t", comment.CommentText ?? "");
                        ins.Parameters.AddWithValue("@c", comment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                        await ins.ExecuteNonQueryAsync();
                        mysqlSaved = true;
                        // Also insert a persisted admin notification so admins see this in the PHP admin feed
                        try
                        {
                            var whoMsg = (await _context.Users.FindAsync(comment.UserId))?.UserName ?? ("User " + comment.UserId);
                            var book = await _context.Books.FindAsync(comment.BookId);
                            var bookTitle = book?.Title ?? ("Book " + comment.BookId);
                            var preview = comment.CommentText ?? "";
                            if (preview.Length > 200) preview = preview.Substring(0, 200) + "...";
                            var notif = mconn.CreateCommand();
                            notif.CommandText = "INSERT INTO admin_notifications (message, created_at, is_read) VALUES (@m,@c,0)";
                            notif.Parameters.AddWithValue("@m", $"User {whoMsg} commented on {bookTitle}: {preview}");
                            notif.Parameters.AddWithValue("@c", comment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                            await notif.ExecuteNonQueryAsync();
                            // After inserting persisted notification, send updated count to admin clients
                            try
                            {
                                var cntCmd = mconn.CreateCommand();
                                cntCmd.CommandText = "SELECT COUNT(*) FROM admin_notifications";
                                var scalar = await cntCmd.ExecuteScalarAsync();
                                int newCount = 0;
                                if (scalar != null && int.TryParse(scalar.ToString(), out var v)) newCount = v;
                                // send count update to admins
                                await _hubContext.Clients.Group("admins").SendAsync("AdminNotificationCount", newCount);
                            }
                            catch (Exception cntEx)
                            {
                                _logger.LogWarning(cntEx, "Failed to compute/send admin notification count");
                            }
                        }
                        catch (Exception nie)
                        {
                            _logger.LogWarning(nie, "Failed to insert admin_notifications row for comment");
                        }
                    }
                }
                catch (Exception mex)
                {
                    _logger.LogWarning(mex, "Failed to mirror comment to MySQL; continuing.");
                }

                if (!sqliteSaved && !mysqlSaved)
                {
                    _logger.LogError("Failed to save comment to both SQLite and MySQL for book {BookId}", bookId);
                    return Json(new { success = false, error = "Failed to save comment", sqliteSaved, mysqlSaved });
                }

                // Notify admins via SignalR
                try
                {
                    var user = await _context.Users.FindAsync(comment.UserId);
                    var book = await _context.Books.FindAsync(comment.BookId);
                    var who = user?.UserName ?? ("User " + comment.UserId);
                    var message = $"{who} commented on {book?.Title ?? ("Book " + comment.BookId)}: { (comment.CommentText?.Length > 120 ? comment.CommentText.Substring(0,120)+"..." : comment.CommentText) }";
                    // Send only to admins group (server-side scoping)
                    await _hubContext.Clients.Group("admins").SendAsync("ReceiveNotification", "Comment", message);
                }
                catch (Exception nex)
                {
                    _logger.LogWarning(nex, "Failed to send comment notification");
                }

                return Json(new { success = true, comment = new { id = comment.CommentId, userId = comment.UserId, text = comment.CommentText, createdAt = comment.CreatedAt }, sqliteSaved, mysqlSaved });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to book {BookId}", bookId);
                return Json(new { success = false, error = "Server error adding comment" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int commentId)
        {
            try
            {
                int? uid = GetCurrentUserId();
                if (uid == null) return Json(new { success = false, error = "Not authenticated" });

                var comment = await _context.Comments.FindAsync(commentId);
                if (comment == null) return Json(new { success = false, error = "Not found" });

                // Allow owner or admin to remove/hide
                var caller = await _context.Users.FindAsync(uid.Value);
                var isAdmin = caller != null && (caller.Role ?? "User").Equals("Admin", StringComparison.OrdinalIgnoreCase);
                if (comment.UserId != uid.Value && !isAdmin) return Json(new { success = false, error = "Forbidden" });

                // Soft-hide in SQLite
                comment.IsHidden = true;
                await _context.SaveChangesAsync();

                // Also delete/hide in MySQL if available
                try
                {
                    var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                    if (!string.IsNullOrEmpty(mySqlConn))
                    {
                        await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                        await mconn.OpenAsync();
                        var upd = mconn.CreateCommand();
                        upd.CommandText = "UPDATE comments SET IsHidden = 1 WHERE CommentId = @id OR (UserId = @u AND BookId = @b) LIMIT 1";
                        upd.Parameters.AddWithValue("@id", commentId);
                        upd.Parameters.AddWithValue("@u", comment.UserId);
                        upd.Parameters.AddWithValue("@b", comment.BookId);
                        await upd.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception mex)
                {
                    _logger.LogWarning(mex, "Failed to mirror comment removal to MySQL; continuing.");
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing comment {CommentId}", commentId);
                return Json(new { success = false });
            }
        }

        private int? GetCurrentUserId()
        {
            var maybeInt = HttpContext.Session.GetInt32("UserId");
            if (maybeInt.HasValue) return maybeInt.Value;
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out var parsed)) return parsed;
            return null;
        }
    }
}
