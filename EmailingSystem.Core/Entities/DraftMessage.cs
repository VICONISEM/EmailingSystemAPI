using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class DraftMessage
    {
        public long Id { get; set; }
        public string? Content { get; set; }
        public long? ConversationId { get; set; }
        public Conversation? Conversation { get; set; }
        public int SenderId { get; set; }
        public ApplicationUser Sender { get; set; } = null!;
        public int? ReceiverId { get; set; }
        public ApplicationUser? Receiver { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get; set; }
        public ICollection<Attachment>? Attachments { get; set; }
    }
}
