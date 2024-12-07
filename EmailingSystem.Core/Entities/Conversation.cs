using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class Conversation
    {
        public long Id { get; set; }
        public string Subject { get; set; } = null!;
        public int SenderId { get; set; }
        public ApplicationUser Sender { get; set; } = null!;
        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } = null!;
        public DateTime SendAt { get; set; } = DateTime.Now;
        public long? LastMessageId { get; set; }
        public Message? LastMessage { get; set; } = null!;


    }
}
