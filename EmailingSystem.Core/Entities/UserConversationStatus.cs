﻿using EmailingSystem.Core.Enums;
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
        public Conversation Conversation { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

    }
}
