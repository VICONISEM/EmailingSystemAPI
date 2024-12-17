using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.PaginatedConversation
{
    public class ConversationDraftSpecificationForCountPagination:BaseSpecification<DraftConversations>
    {
        public ConversationDraftSpecificationForCountPagination(ConversationSpecParams Specs , int UserId) 
        {

            Criteria = D => D.SenderId == UserId && (string.IsNullOrEmpty(Specs.Search) || D.Subject.Trim().ToLower().Contains(Specs.Search));

        }
    }
}
