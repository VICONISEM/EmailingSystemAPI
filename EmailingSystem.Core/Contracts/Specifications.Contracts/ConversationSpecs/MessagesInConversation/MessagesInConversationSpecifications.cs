using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.MessagesInConversation
{
    public class MessagesInConversationSpecifications : BaseSpecification<Conversation>
    {
        public MessagesInConversationSpecifications(ConversationWithMessagesSpecsParams Specs, int userId)
        {

            Criteria = C => C.Id == Specs.ConversationId;

            AddInclude(In => In.Include(M=>M.Messages.Where(m =>( 
            (m.SenderId == userId && !m.SenderIsDeleted)
            ||
            (m.ReceiverId == userId && !m.ReceiverIsDeleted))&&!m.IsDraft)).ThenInclude(A=>A.Attachments));


            

           
  
        }
    }
}
