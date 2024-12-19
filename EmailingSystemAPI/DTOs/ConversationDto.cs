using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs
{
    public class ConversationDto
    {
        public long Id { get; set; }
        public string Subject { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public LastMessageDto LastMessage { get; set; } = null!;

    }
}
