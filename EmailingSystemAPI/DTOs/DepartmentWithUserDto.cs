namespace EmailingSystemAPI.DTOs
{
    public class DepartmentWithUserDto
    {
        public string Name { get; set; } = null!;
        public string Abbreviation { get; set; } = null!;
        public string CollegeName { get; set; } = null!;

        public int userId { get; set; }
    }
}
