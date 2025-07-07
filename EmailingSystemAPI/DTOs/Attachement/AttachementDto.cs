namespace EmailingSystemAPI.DTOs.Attachement
{
    public class AttachementDto
    {
        public int Id { get; set; }
        public string FileURL { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Size { get; set; }
    }
}
