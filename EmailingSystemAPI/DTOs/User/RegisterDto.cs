using System.ComponentModel.DataAnnotations;

namespace EmailingSystemAPI.DTOs.User
{
    public class RegisterDto
    {
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;

        [MinLength(8)]
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int DepartmentId { get; set; }

        [MinLength(14)]
        public string NationalId { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public IFormFile? Picture { get; set; }
        public int? SignatureId { get; set; }
        public IFormFile? Signature { get; set; }

    }
}
