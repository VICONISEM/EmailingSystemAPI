using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class Message
    {
        public long Id { get; set; }
        public string? Content { get; set; } = null!;
        public long ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
        public int SenderId { get; set; }
        public ApplicationUser Sender { get; set; } = null!;
        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime SendAt { get; set; } = DateTime.Now;
        public DateTime ReceivedAt { get; set; }
        public long? ParentMessageId { get; set; }
        public Message? ParentMessage { get; set; } = null!;
        public ICollection<Attachment>? Attachments { get; set; }
        public bool SenderIsDeleted { get; set; }
        public bool ReceiverIsDeleted { get; set; }
        public bool IsDraft { get; set; } = false;


    }
}
