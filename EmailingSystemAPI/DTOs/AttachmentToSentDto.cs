using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs
{
    public class AttachmentToSentDto
    {
        public string FileName { get; set; } = null!;
        public long MessageId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
