namespace EmailingSystemAPI.DTOs.User
{
    public class AuthDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? PictureURL { get; set; }
        public string? SignatureURL { get; set; }
        public string? DepartmentName { get; set; } = null!;
        public string? CollegeName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string? AccessToken { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }
    }
}