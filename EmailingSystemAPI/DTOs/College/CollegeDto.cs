using EmailingSystemAPI.DTOs.Department;

namespace EmailingSystemAPI.DTOs.College
{
    public class CollegeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Abbreviation { get; set; }

        public ICollection<DepartmentDto> ? Departments { get; set; }
    }
}
