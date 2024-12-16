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
        public ApplicationUser Sender { get; set; } = null!;
        public int? ReceiverId { get; set; }
        public ApplicationUser? Receiver { get; set; } = null!;
        public string? Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<DraftAttachments>? DraftAttachments  { get; set; }






        
    }
}
