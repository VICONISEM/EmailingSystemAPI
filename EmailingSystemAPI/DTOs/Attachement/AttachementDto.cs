namespace EmailingSystemAPI.DTOs.Attachement
{
    public class AttachementDto
    {
        public string FileURL { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Size { get; set; }
    }
}
