using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcReadMe_Group4.Data;
using MvcReadMe_Group4.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcReadMe_Group4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly MvcReadMe_Group4Context _context;

        public class TopBookNotification
        {
            public required int BookId { get; set; }
            public required string Title { get; set; }
            public required int TotalReads { get; set; }
            public required string TimeAgo { get; set; }
            public string? CoverImagePath { get; set; }
        }

        public NotificationsController(MvcReadMe_Group4Context context)
        {
            _context = context;
        }

        [HttpGet("most-read")]
        public async Task<IActionResult> GetMostReadBooks()
        {
            var mostReadBooks = await _context.Books
                .Select(b => new TopBookNotification
                {
                    BookId = b.Id,
                    Title = b.Title,
                    TotalReads = _context.BookReads.Count(br => br.ReadCount > 0),
                    TimeAgo = "Updated recently", // We'll calculate this dynamically
                    CoverImagePath = b.CoverImagePath
                })
                .OrderByDescending(b => b.TotalReads)
                .Take(5)
                .ToListAsync();

            // Calculate time ago for each book
            foreach (var book in mostReadBooks)
            {
                var latestRead = await _context.BookReads
                    .OrderByDescending(br => br.ReadDate)
                    .FirstOrDefaultAsync();

                if (latestRead != null)
                {
                    book.TimeAgo = GetTimeAgo(latestRead.ReadDate);
                }
            }

            return Ok(mostReadBooks);
        }

        private string GetTimeAgo(DateTime date)
        {
            var timeSpan = DateTime.Now - date;

            if (timeSpan.TotalMinutes < 1)
                return "just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} minutes ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} hours ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} days ago";
            
            return date.ToString("MMM dd, yyyy");
        }
    }
}