using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs
{
    public class DraftSpecification :BaseSpecification<DraftConversations>
    {
        public DraftSpecification(ConversationSpecParams specs, int UserId):base ()
        {
            Criteria = D => D.SenderId == UserId && string.IsNullOrEmpty(specs.Search) || D.Subject.Trim().ToLower().Contains(specs.Search);

            
            
            
               
            ApplyPagination(specs.PageSize * (specs.PageNumber - 1), specs.PageSize);


        }

    }
}
