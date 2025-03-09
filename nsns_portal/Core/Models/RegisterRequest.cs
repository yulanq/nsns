
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
