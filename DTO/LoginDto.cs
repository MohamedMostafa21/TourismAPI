using System.ComponentModel.DataAnnotations;

namespace TourismAPI.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; } = false;

    }
}
