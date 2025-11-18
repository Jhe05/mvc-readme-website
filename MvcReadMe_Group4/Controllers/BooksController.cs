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
    public class BooksController : Controller
    {
        private readonly MvcReadMe_Group4Context _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger<BooksController> _logger;

        public BooksController(MvcReadMe_Group4Context context, IHubContext<NotificationHub> hubContext, Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Logging.ILogger<BooksController> logger)
        {
            _context = context;
            _hubContext = hubContext;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: /Books/Read/5
        public async Task<IActionResult> Read(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Increment total reads
            book.NumberOfReads++;

            // Get or create today's BookRead record for this specific book
            var today = DateTime.Today;
            var bookRead = await _context.BookReads
                .FirstOrDefaultAsync(br => br.BookId == id && br.ReadDate == today);

            if (bookRead == null)
            {
                bookRead = new BookRead
                {
                    BookId = id,
                    ReadDate = today,
                    ReadCount = 1
                };
                _context.BookReads.Add(bookRead);
            }
            else
            {
                bookRead.ReadCount++;
            }
            await _context.SaveChangesAsync();

            // Dual-write to MySQL if configured so PHP reports (phpMyAdmin) reflect app-side reads
            try
            {
                var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                if (!string.IsNullOrEmpty(mySqlConn))
                {
                    await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                    await mconn.OpenAsync();

                    // Ensure the books.NumberOfReads is incremented in MySQL as well
                    try
                    {
                        var updBooks = mconn.CreateCommand();
                        updBooks.CommandText = "UPDATE books SET NumberOfReads = NumberOfReads + 1 WHERE Id = @b";
                        updBooks.Parameters.AddWithValue("@b", id);
                        await updBooks.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to increment books.NumberOfReads in MySQL for book {BookId}", id);
                    }

                    // Upsert today's BookRead row in MySQL (match by BookId and date prefix)
                    var datePrefix = today.ToString("yyyy-MM-dd");
                    var sel = mconn.CreateCommand();
                    sel.CommandText = "SELECT Id, ReadCount FROM BookReads WHERE BookId = @b AND ReadDate LIKE CONCAT(@p, '%') LIMIT 1";
                    sel.Parameters.AddWithValue("@b", id);
                    sel.Parameters.AddWithValue("@p", datePrefix);
                    await using var reader = await sel.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        var existingId = reader.GetInt32(0);
                        reader.Close();
                        var upd = mconn.CreateCommand();
                        upd.CommandText = "UPDATE BookReads SET ReadCount = ReadCount + 1 WHERE Id = @id";
                        upd.Parameters.AddWithValue("@id", existingId);
                        await upd.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        reader.Close();
                        var ins = mconn.CreateCommand();
                        ins.CommandText = "INSERT INTO BookReads (BookId, ReadDate, ReadCount) VALUES (@b,@d,@c)";
                        ins.Parameters.AddWithValue("@b", id);
                        ins.Parameters.AddWithValue("@d", datePrefix);
                        ins.Parameters.AddWithValue("@c", 1);
                        await ins.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to mirror BookRead to MySQL; continuing with SQLite only.");
            }
            // Notify connected clients about updated read count so UI can update in real-time
            try
            {
                await _hubContext.Clients.All.SendAsync("BookReadUpdated", id, book.NumberOfReads);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Failed to broadcast BookReadUpdated");
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncrementReads(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            // Get current username
            var username = User.Identity?.Name ?? "Anonymous";

            // Increment reads
            book.NumberOfReads++;
            await _context.SaveChangesAsync();

            // Mirror NumberOfReads increment to MySQL if configured
            try
            {
                var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                if (!string.IsNullOrEmpty(mySqlConn))
                {
                    await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                    await mconn.OpenAsync();
                    var updBooks = mconn.CreateCommand();
                    updBooks.CommandText = "UPDATE books SET NumberOfReads = NumberOfReads + 1 WHERE Id = @b";
                    updBooks.Parameters.AddWithValue("@b", bookId);
                    await updBooks.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to increment books.NumberOfReads in MySQL for book {BookId}", bookId);
            }

            // Send notification to admins via SignalR group
            try
            {
                await _hubContext.Clients.Group("admins").SendAsync("ReceiveNotification",
                    "Reading Activity",
                    $"{username} just started reading {book.Title}!");
                // Also broadcast the updated read count to all connected clients so notification counts update
                try
                {
                    await _hubContext.Clients.All.SendAsync("BookReadUpdated", bookId, book.NumberOfReads);
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Failed to broadcast BookReadUpdated from IncrementReads");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to notify admins about reading activity");
            }

            return Ok();
        }
    }
}