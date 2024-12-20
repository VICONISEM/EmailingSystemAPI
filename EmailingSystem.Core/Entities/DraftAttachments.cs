using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class DraftAttachments
    {
        public int Id { get; set; }
        public long DraftConversationId { get; set; }
        public virtual DraftConversations draftConversations { get; set; }
        public string AttachmentPath { get; set; }

    }
}
