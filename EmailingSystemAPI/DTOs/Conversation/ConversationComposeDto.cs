using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Message;

namespace EmailingSystemAPI.DTOs.Conversation
{
    public class ConversationComposeDto
    {

        public string Subject { get; set; } = null!;
        public int ReceiverId { get; set; }
        public MessageSendComposeDto Message { get; set; } = null!;


    }
}
