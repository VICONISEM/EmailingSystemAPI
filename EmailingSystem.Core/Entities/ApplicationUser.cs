﻿using EmailingSystem.Core.Entities.Token;
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
        public virtual Department? Department { get; set; } = null!;
        public int? CollegeId { get; set; }
        public virtual College? College { get; set; } = null!;
        public int? SignatureId { get; set; }
        public virtual Signature? Signature { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Conversation> ConversationsSender { get; set; } = null!;
        public virtual ICollection<Conversation> ConversationsReceiver { get; set; } = null!;
        public virtual ICollection<Message> MessagesSender { get; set; } = null!;
        public virtual ICollection<Message> MessagesReceiver { get; set; } = null!;
        public virtual ICollection<DraftConversations> DraftsSender { get; set; } = null!;
        public virtual ICollection<DraftConversations> DraftsReceiver { get; set; } = null!;
        public virtual ICollection<UserConversationStatus> UserConversationStatuses { get; set; } = null!;
       
        public virtual HashSet<RefreshToken>? RefreshTokens { get; set; } = new HashSet<RefreshToken>();

    }
}
