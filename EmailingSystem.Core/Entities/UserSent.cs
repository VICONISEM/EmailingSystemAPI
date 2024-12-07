using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public  class UserSent
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public long ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
        
    }
}
