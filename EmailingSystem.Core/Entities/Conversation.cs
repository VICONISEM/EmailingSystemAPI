namespace EmailingSystem.Core.Entities
{
    public class Conversation
    {
        public long Id { get; set; }
        public string Subject { get; set; } = null!;
        public int SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; } = null!;
        public int ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; } = null!;
        public virtual ICollection<Message> Messages { get; set; } = null!;
        public virtual ICollection<UserConversationStatus> UserConversationStatuses { get; set; } = null!;
      
    }
}
