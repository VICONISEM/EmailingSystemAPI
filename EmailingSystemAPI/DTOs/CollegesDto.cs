namespace EmailingSystemAPI.DTOs
{
    public class CollegesDto
    {
        public int Id { get; set; }
        public string? Name { get; set;}

        public string? Abbreviation { get; set; }

        public List<DepartmentDto>? Departments { get; set; }
    }
}
