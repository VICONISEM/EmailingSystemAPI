using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.MessagesInConversation
{
    public class GetDraftMessageSpecification : BaseSpecification<Conversation>
    {
        public GetDraftMessageSpecification(int UserId,long ConversationId) 
        {
            Criteria = C => (C.Id == ConversationId);
            AddInclude(In => In.Include(C => C.Messages.Where(M => M.SenderId == UserId && M.IsDraft && !M.SenderIsDeleted )));


        }
    }
}
