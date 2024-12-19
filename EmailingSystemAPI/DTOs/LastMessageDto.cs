using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs
{
    public class LastMessageDto
    {
        public string? Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; }
        public IEnumerable<AttachementDto> Attachements { get; set; } = new List<AttachementDto>();

    }
}
