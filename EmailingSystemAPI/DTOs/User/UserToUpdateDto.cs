using EmailingSystem.Core.Enums;

namespace EmailingSystemAPI.DTOs.User
{
    public class UserToUpdateDto
    {
        public string Name { get; set; } = null!;
        public UserRole Role { get; set; }
        public IFormFile Signature { get; set; }
        public IFormFile Picture { get; set; }
        public string NationalId { get; set; } = null!;
        public int? CollegeId { get; set; } = null;
        public int? DepartmentId { get; set; } = null;
    }
}
