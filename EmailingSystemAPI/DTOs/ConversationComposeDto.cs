using EmailingSystem.Core.Entities;

namespace EmailingSystemAPI.DTOs
{
    public class ConversationComposeDto
    {
      
        public string Subject { get; set; } = null!;
        public int ReceiverId { get; set; }
        public MessageSendComposeDto Message { get; set; } = null!;
       

    }
}
