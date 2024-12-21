using EmailingSystem.Core.Enums;
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
        public int? DepartmentId { get; set; } = 0;
        public int? CollegeId { get; set; } = 0;
        public UserRole Role { get; set; } = UserRole.NormalUser;

        [MinLength(14)]
        [MaxLength(14)]
        [RegularExpression(@"^\d{14}$")]
        public string NationalId { get; set; } = null!;

        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only image files (.jpg, .jpeg, .png) are allowed.")]
        public IFormFile? Picture { get; set; }

        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only image files (.jpg, .jpeg, .png) are allowed.")]
        public IFormFile? SignatureFile { get; set; }


    }
}
