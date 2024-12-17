using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.MessagesInConversation
{
    public class MessagesInConversationSpecifications : BaseSpecification<Message>
    {
        public MessagesInConversationSpecifications(MessagesInConversationSpecParams Specs)
        {
            ApplyPagination()
        }
    }
}
