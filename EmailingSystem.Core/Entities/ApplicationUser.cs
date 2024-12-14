using EmailingSystem.Core.Entities.Token;
using Microsoft.AspNetCore.Identity;
namespace EmailingSystem.Core.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; } = null!;

        private string NormalizedNameAttribute = null!;

        public string NormalizedName
        {
            get { return NormalizedNameAttribute; }
            set { NormalizedNameAttribute = Name.Trim().ToUpper();}
        }

        public string NationalId { get; set; } = null!;
        public string? PicturePath { get; set; } = null!;
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; } = null!;
        public int? CollegeId { get; set; }
        public College? College { get; set; } = null!;
        public int? SignatureId { get; set; }
        public Signature? Signature { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Conversation> ConversationsSender { get; set; } = null!;
        public ICollection<Conversation> ConversationsReceiver { get; set; } = null!;
        public ICollection<Message> MessagesSender { get; set; } = null!;
        public ICollection<Message> MessagesReceiver { get; set; } = null!;
        public ICollection<Draft> DraftsSender { get; set; } = null!;
        public ICollection<Draft> DraftsReceiver { get; set; } = null!;
        public ICollection<UserConversationStatus> UserConversationStatuses { get; set; } = null!;
        public ICollection<UserInbox> UserInboxes { get; set; } = null!;
        public ICollection<UserSent> UserSents { get; set; } = null!;
        public HashSet<RefreshToken>? RefreshTokens { get; set; } = new HashSet<RefreshToken>();

    }
}
