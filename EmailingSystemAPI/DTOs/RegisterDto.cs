using System.ComponentModel.DataAnnotations;

namespace EmailingSystemAPI.DTOs
{
    public class RegisterDto
    {
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int? SignatureId { get; set; }
        public string NationalId { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public IFormFile? File { get; set; } 
        
    }
}
