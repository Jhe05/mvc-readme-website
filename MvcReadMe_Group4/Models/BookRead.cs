using System;
using System.ComponentModel.DataAnnotations;

namespace MvcReadMe_Group4.Models
{
    public class BookRead
    {
        public int Id { get; set; }

        [Required]
        public DateTime ReadDate { get; set; }

        [Required]
        public int ReadCount { get; set; }
        
        // Foreign key to the Book this read entry refers to. Database migrations
        // created a non-nullable BookId column, so the model must expose it
        // and the controller must set it when creating BookRead records.
        public int BookId { get; set; }
        
        // Navigation property (optional) so EF can load the related Book.
        public Book? Book { get; set; }
    }
}