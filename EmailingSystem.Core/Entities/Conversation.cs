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
        public long? LastMessageId { get; set; }
        public Message? LastMessage { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = null!;
        public ICollection<UserConversationStatus> UserConversationStatuses { get; set; } = null!;
        public ICollection<UserInbox> UserInboxes { get; set; } = null!;
        public ICollection<UserSent> UserSents { get; set; } = null!;
    }
}
