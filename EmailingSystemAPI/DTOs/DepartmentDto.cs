namespace EmailingSystemAPI.DTOs
{
    public class DepartmentDto
    {
        public string Name { get; set; } = null!;
        public string Abbreviation { get; set; } = null!;
        public int CollegeId { get; set; }
    }
}
