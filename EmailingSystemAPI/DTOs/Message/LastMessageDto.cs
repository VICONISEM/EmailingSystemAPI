using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;

namespace EmailingSystemAPI.DTOs.Message
{
    public class LastMessageDto
    {
        public string? Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }
        public IEnumerable<AttachementDto> Attachements { get; set; } = new List<AttachementDto>();

    }
}
