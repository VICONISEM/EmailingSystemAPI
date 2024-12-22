using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;

namespace EmailingSystemAPI.DTOs.Message
{
    public class MessageSendComposeDto
    {
        public string? Content { get; set; } = null!;  
        public ICollection<AttachmentToSentDto>? Attachments { get; set; }

    }
}
