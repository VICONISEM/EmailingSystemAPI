using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;

namespace EmailingSystemAPI.DTOs.DraftConversation
{
    public class DraftComposeDto
    {
        public string? Subject { get; set; } = null!;
        public int? ReceiverId { get; set; }
        public string? Body { get; set; }
        public List<AttachmentDraftDto>? DraftAttachments { get; set; }
    }
}
