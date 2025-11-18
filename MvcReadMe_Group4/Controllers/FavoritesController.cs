using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcReadMe_Group4.Data;
using MvcReadMe_Group4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace MvcReadMe_Group4.Controllers
{
	public class FavoritesController : Controller
	{
	private readonly MvcReadMe_Group4Context _context;
	private readonly ILogger<FavoritesController> _logger;
	private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

		public FavoritesController(MvcReadMe_Group4Context context, ILogger<FavoritesController> logger, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			_context = context;
			_logger = logger;
			_configuration = configuration;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(int bookId)
		{
			try
			{
				int? userId = GetCurrentUserId();
				if (userId == null)
					return Json(new { success = false, error = "Not authenticated" });

				var exists = await _context.Favorites
					.AsNoTracking()
					.AnyAsync(f => f.BookId == bookId && f.UserId == userId.Value);

				Favorite? addedFavorite = null;
				if (!exists)
				{
					var fav = new Favorite
					{
						BookId = bookId,
						UserId = userId.Value,
						CreatedAt = DateTime.UtcNow
					};

					_context.Favorites.Add(fav);
					await _context.SaveChangesAsync();
					addedFavorite = fav;
				}

				// Dual-write to MySQL if configured
				try
				{
					var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
					if (!string.IsNullOrEmpty(mySqlConn))
					{
						// Use MySqlConnector to insert a matching row
						await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
						await mconn.OpenAsync();
						var insertCmd = mconn.CreateCommand();
						// table created by EF migrations uses PascalCase column names in MySQL (FavoriteId, UserId, BookId, CreatedAt)
						insertCmd.CommandText = "INSERT INTO favorites (UserId, BookId, CreatedAt) VALUES (@u,@b,@c)";
						insertCmd.Parameters.AddWithValue("@u", userId.Value);
						insertCmd.Parameters.AddWithValue("@b", bookId);
						insertCmd.Parameters.AddWithValue("@c", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
						await insertCmd.ExecuteNonQueryAsync();
					}
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Failed to write favorite to MySQL; continuing with SQLite only.");
				}

				var newCount = await _context.Favorites.CountAsync(f => f.UserId == userId.Value);

				object? favInfo = null;
				if (addedFavorite != null)
				{
					var book = await _context.Books.FindAsync(bookId);
					favInfo = new { bookId = book?.Id, title = book?.Title, coverImagePath = book?.CoverImagePath };
				}

				return Json(new { success = true, count = newCount, favorite = favInfo });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding favorite for book {BookId}", bookId);
				return Json(new { success = false, error = "Server error adding favorite" });
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Remove(int bookId)
		{
			try
			{
				int? userId = GetCurrentUserId();
				if (userId == null)
					return Json(new { success = false, error = "Not authenticated" });

				var fav = await _context.Favorites
					.FirstOrDefaultAsync(f => f.BookId == bookId && f.UserId == userId.Value);

				object? removedInfo = null;
				if (fav != null)
				{
					// capture book info before removing for client-side removal
					var book = await _context.Books.FindAsync(bookId);
					removedInfo = new { bookId = book?.Id };

					_context.Favorites.Remove(fav);
					await _context.SaveChangesAsync();
				}

				// Dual-delete from MySQL if configured
				try
				{
					var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
					if (!string.IsNullOrEmpty(mySqlConn))
					{
						await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
						await mconn.OpenAsync();
						var delCmd = mconn.CreateCommand();
						// match PascalCase column names used by EF-created MySQL table
						delCmd.CommandText = "DELETE FROM favorites WHERE UserId = @u AND BookId = @b";
						delCmd.Parameters.AddWithValue("@u", userId.Value);
						delCmd.Parameters.AddWithValue("@b", bookId);
						await delCmd.ExecuteNonQueryAsync();
					}
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Failed to delete favorite from MySQL; continuing with SQLite only.");
				}

				var newCount = await _context.Favorites.CountAsync(f => f.UserId == userId.Value);
				return Json(new { success = true, count = newCount, removed = removedInfo });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error removing favorite for book {BookId}", bookId);
				return Json(new { success = false, error = "Server error removing favorite" });
			}
		}

		[HttpGet]
		public async Task<IActionResult> IsFavorite(int bookId)
		{
			try
			{
				int? userId = GetCurrentUserId();
				if (userId == null)
					return Json(new { isFavorite = false });

				var exists = await _context.Favorites
					.AsNoTracking()
					.AnyAsync(f => f.BookId == bookId && f.UserId == userId.Value);

				return Json(new { isFavorite = exists });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error checking IsFavorite for book {BookId}", bookId);
				return Json(new { isFavorite = false });
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				int? userId = GetCurrentUserId();
				if (userId == null)
					return Json(new { success = false });

				var favorites = await _context.Favorites
					.AsNoTracking()
					.Include(f => f.Book)
					.Where(f => f.UserId == userId.Value)
					.Select(f => new {
						bookId = f.Book!.Id,
						title = f.Book!.Title,
						coverImagePath = f.Book!.CoverImagePath
					})
					.ToListAsync();

				return Json(new { success = true, favorites });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching favorites for user");
				return Json(new { success = false });
			}
		}

		[HttpGet]
		public IActionResult MyFavorites()
		{
			return View();
		}

		// Temporary debug endpoints (no antiforgery) to help diagnose client/server issues.
		// Remove these before deploying to production.
		[HttpPost]
		public async Task<IActionResult> AddNoAuth(int bookId)
		{
			try
			{
				int? userId = GetCurrentUserId();
				if (userId == null)
					return Json(new { success = false, error = "Not authenticated" });

				var exists = await _context.Favorites
					.AsNoTracking()
					.AnyAsync(f => f.BookId == bookId && f.UserId == userId.Value);

				if (!exists)
				{
					var fav = new Favorite { BookId = bookId, UserId = userId.Value, CreatedAt = DateTime.UtcNow };
					_context.Favorites.Add(fav);
					await _context.SaveChangesAsync();
				}

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "AddNoAuth error for book {BookId}", bookId);
				return Json(new { success = false, error = "Server error" });
			}
		}

		[HttpPost]
		public async Task<IActionResult> RemoveNoAuth(int bookId)
		{
			try
			{
				int? userId = GetCurrentUserId();
				if (userId == null)
					return Json(new { success = false, error = "Not authenticated" });

				var fav = await _context.Favorites.FirstOrDefaultAsync(f => f.BookId == bookId && f.UserId == userId.Value);
				if (fav != null)
				{
					_context.Favorites.Remove(fav);
					await _context.SaveChangesAsync();
				}

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "RemoveNoAuth error for book {BookId}", bookId);
				return Json(new { success = false, error = "Server error" });
			}
		}
		[HttpGet]
		public IActionResult Diag()
		{
			// Diagnostic endpoint to help debug session and antiforgery cookie presence
			var sessionIdInt = HttpContext.Session.GetInt32("UserId");
			var sessionIdStr = HttpContext.Session.GetString("UserId");
			var cookies = Request.Cookies.Keys.ToList();

			return Json(new
			{
				sessionInt = sessionIdInt,
				sessionStr = sessionIdStr,
				cookies = cookies
			});
		}

		private int? GetCurrentUserId()
		{
			// Try session int first
			var maybeInt = HttpContext.Session.GetInt32("UserId");
			if (maybeInt.HasValue) return maybeInt.Value;

			// Fallback to string parse if code stored as string
			var userIdStr = HttpContext.Session.GetString("UserId");
			if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out var parsed))
				return parsed;

			return null;
		}
	}
}
