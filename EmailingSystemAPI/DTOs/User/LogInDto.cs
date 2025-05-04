using System.ComponentModel.DataAnnotations;

namespace EmailingSystemAPI.DTOs.User
{
    public class LogInDto
    {
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
