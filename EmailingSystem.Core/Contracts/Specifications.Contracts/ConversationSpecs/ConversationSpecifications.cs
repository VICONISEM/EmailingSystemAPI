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
    public class ConversationSpecifications : BaseSpecification<Conversation>
    {
        public ConversationSpecifications(ConversationSpecParams Specs, int UserId)
        {
            Criteria = C => C.SenderId == UserId || C.ReceiverId == UserId
            &&
            string.IsNullOrEmpty(Specs.Search) || C.Subject.ToLower().Contains(Specs.Search)
            |
            (C.SenderId == UserId || C.Sender.NormalizedName.ToLower().Contains(Specs.Search))
            |
            (C.ReceiverId == UserId || C.Sender.NormalizedName.ToLower().Contains(Specs.Search));

            AddInclude(C => C.Include(C => C.UserConversationStatuses.Where(C => C.UserId == UserId)));

            if (Specs.Sort == "dsec")
                OrderByDesc = (C => C.Messages.Where(C => C.SenderId == UserId && !C.SenderIsDeleted || C.ReceiverId == UserId && !C.ReceiverIsDeleted).Max(M => M.SendAt));
                
            else
                OrderBy = (C => C.Messages.Where(C => C.SenderId == UserId && !C.SenderIsDeleted || C.ReceiverId == UserId && !C.ReceiverIsDeleted).Max(M => M.SendAt));


            ApplyPagination(Specs.PageSize * (Specs.PageNumber - 1), Specs.PageSize);
        }
    }
}
