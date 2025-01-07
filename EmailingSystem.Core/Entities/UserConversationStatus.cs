using EmailingSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class UserConversationStatus
    {
        public int UserId { get; set; }
        public long ConversationId { get; set; }
        public ConversationStatus Status { get; set; } = ConversationStatus.Active;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public virtual Conversation Conversation { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

    }
}

