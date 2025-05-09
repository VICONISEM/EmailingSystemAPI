namespace EmailingSystemAPI.DTOs.User
{
    public class AllowedUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? PictureURL { get; set; } = null!;
        public string? CollegeName { get; set; } = null!;
        public string? DepartmentName { get; set; } = null!;
    }
}
