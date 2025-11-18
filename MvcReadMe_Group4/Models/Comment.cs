using System;
using System.ComponentModel.DataAnnotations;

namespace MvcReadMe_Group4.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        // FK to User.Id
        public int UserId { get; set; }

        // FK to Book.Id
        public int BookId { get; set; }

        // The comment body
        public string? CommentText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Moderation flag (soft-hide)
        public bool IsHidden { get; set; } = false;

        // Navigation
        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}
