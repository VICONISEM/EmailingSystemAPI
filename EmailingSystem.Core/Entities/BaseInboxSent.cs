namespace EmailingSystem.Core.Entities
{
    public class BaseInboxSent
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public long ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
    }
}