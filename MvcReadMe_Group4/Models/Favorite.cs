using System;
using System.ComponentModel.DataAnnotations;

namespace MvcReadMe_Group4.Models
{
	public class Favorite
	{
		[Key]
		public int FavoriteId { get; set; }

		// FK to User.Id
		public int UserId { get; set; }

		// FK to Book.Id
		public int BookId { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation properties (optional)
		public User? User { get; set; }
		public Book? Book { get; set; }
	}
}
