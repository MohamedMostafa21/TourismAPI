using System.ComponentModel.DataAnnotations;

namespace TourismAPI.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; init; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? Password { get; init; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(20, ErrorMessage = "First name cannot exceed 20 characters")]
        public string? FirstName { get; init; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(20, ErrorMessage = "Last name cannot exceed 20 characters")]
        public string? LastName { get; init; }

        [Range(18, 70, ErrorMessage = "Age must be between 18 and 70")]
        public int Age { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'")]
        public string Gender { get; set; }

        public string? Country { get; set; }

        // Profile photo file validation
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPG, JPEG, and PNG files are allowed")]
        public IFormFile? ProfilePhoto { get; set; }

        // CV file validation
        //[FileExtensions(Extensions = "pdf", ErrorMessage = "Only PDF files are allowed")]
        public IFormFile? CvDocument { get; set; }

        public string? Role { get; set; }

        public bool rememberMe { get; set; } = false;

       
    }
}