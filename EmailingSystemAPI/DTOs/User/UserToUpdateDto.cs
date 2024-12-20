namespace EmailingSystemAPI.DTOs.User
{
    public class UserToUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Role { get; set; }
        public string? PicturePath { get; set; } = null!;
        public IFormFile? Picture { get; set; }
        public string NationalId { get; set; } = null!;
        public int? CollegeId { get; set; } = null!;
        public int? DepartmentId { get; set; } = null!;
    }
}
