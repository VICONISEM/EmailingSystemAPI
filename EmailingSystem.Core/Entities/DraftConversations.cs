using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class DraftConversations
    {
        public long Id { get; set; }
        public string? Subject { get; set; } = null!;
        public int SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; } = null!;
        public int? ReceiverId { get; set; }
        public virtual ApplicationUser? Receiver { get; set; } = null!;
        public string? Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual List<DraftAttachments>? DraftAttachments  { get; set; }






        
    }
}
