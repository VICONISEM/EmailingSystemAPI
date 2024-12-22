using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;

namespace EmailingSystemAPI.DTOs.DraftConversation
{
    public class DraftConversationDtoReturn
    {

        public long Id { get; set; }
        public string? Subject { get; set; } = null!;
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public string? Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<AttachementDto>? DraftAttachments { get; set; }



    }
}
