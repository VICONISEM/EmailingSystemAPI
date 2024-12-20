namespace EmailingSystemAPI.DTOs.User
{
    public class AuthDto
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public int? SignatureId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string CollegeName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string? AccessToken { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }
    }
}