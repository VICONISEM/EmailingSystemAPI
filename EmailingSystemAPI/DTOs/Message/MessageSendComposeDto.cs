using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;

namespace EmailingSystemAPI.DTOs.Message
{
    public class MessageSendComposeDto
    {
        public string? Content { get; set; } = null!;
        public long ConversationId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public ICollection<AttachmentToSentDto>? Attachments { get; set; }

    }
}
