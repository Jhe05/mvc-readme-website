using System.ComponentModel.DataAnnotations;

namespace MvcReadMe_Group4.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]       
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        private string _password = "";
        public string Password 
        { 
            get { return _password; }
            set { _password = value; }
        }

        [Required]
        private string _role = "User";
        public string Role 
        { 
            get { return _role; }
            set { _role = value; }
        }

        public string? AvatarPath { get; set; }
    }
}
