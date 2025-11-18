using System.ComponentModel.DataAnnotations;

namespace MvcReadMe_Group4.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        private string _filePath = string.Empty;
        public string FilePath 
        { 
            get { return _filePath; }
            set { _filePath = value; }
        }

        private string _coverImagePath = string.Empty;
        public string CoverImagePath 
        { 
            get { return _coverImagePath; }
            set { _coverImagePath = value; }
        }

        private int _numberOfReads;
        public int NumberOfReads 
        { 
            get { return _numberOfReads; }
            set { _numberOfReads = value; }
        }
    }
}
