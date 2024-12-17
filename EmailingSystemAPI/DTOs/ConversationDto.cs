using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs
{
    public class ConversationDto
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public DateTime LastMessageTime { get; set; }
        public LastMessageDto LastMessage { get; set; }
        public bool IsOpened { get; set; }
    }
}
