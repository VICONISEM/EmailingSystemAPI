using System.ComponentModel.DataAnnotations;

namespace EmailingSystemAPI.DTOs.User
{
    public class ChangePasswordDto
    {
        [MinLength(8)]
        public string OldPassword { get; set; } = null!;
        [MinLength(8)]
        public string NewPassword { get; set; } = null!;
    }
}
