using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs
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
