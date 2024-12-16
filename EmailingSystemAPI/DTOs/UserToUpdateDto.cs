namespace EmailingSystemAPI.DTOs
{
    public class UserToUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Role { get; set; }
        public string? PicturePath { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string? CollegeId { get; set; } = null!;
        public string? DepartmentId { get; set; } = null!;
    }
}
