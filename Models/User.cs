using Microsoft.AspNetCore.Identity;

namespace TourismAPI.Models
{
    public class User:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public string? CvUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? Role { get; set; }
    }
}
