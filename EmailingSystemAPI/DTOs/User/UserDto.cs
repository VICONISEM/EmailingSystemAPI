namespace EmailingSystemAPI.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Role { get; set; }
        public string? PictureURL { get; set; } = null!;
        public string? SignatureURL { get; set; } = null!;
        public string? CollegeName { get; set; } = null!;
        public int ? CollegeId { get; set; }
        public string? DepartmentName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
    }
}