﻿using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Message;
using System.Runtime.CompilerServices;

namespace EmailingSystemAPI.DTOs.Conversation
{
    public class ConversationDto
    {
        public long Id { get; set; }
        public string Subject { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string ?SenderPictureURL { get; set; } = null;
        public string ReceiverEmail { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public string ?ReceiverPictureURL { get; set; } = null;
        public LastMessageDto LastMessage { get; set; } = null!;
        public bool HasDraftMessage { get; set; } = false;

    }
}
