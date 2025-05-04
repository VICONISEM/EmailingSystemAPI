using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs.Attachement
{
    public class AttachmentToSentDto
    {
        public string FileName { get; set; } = null!;
        public long MessageId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
