using System.ComponentModel.DataAnnotations;

namespace TaskManagerApplication.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        
    }
}
