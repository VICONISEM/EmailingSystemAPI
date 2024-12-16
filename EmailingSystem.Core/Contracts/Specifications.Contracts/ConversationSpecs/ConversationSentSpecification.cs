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
    public class ConversationSentSpecification : BaseSpecification<Conversation>
    {

        public ConversationSentSpecification(ConversationSpecParams Specs, int UserId)
        {
            Criteria = C => ((C.ReceiverId == UserId || C.SenderId == UserId) && C.Messages.Any(M => M.SenderId == UserId))
               &&
               (string.IsNullOrEmpty(Specs.Search) ||
               (C.Subject.ToLower().Contains(Specs.Search)
               ||
               C.SenderId == UserId || C.Sender.NormalizedName.ToLower().Contains(Specs.Search)
               ||
               C.ReceiverId == UserId || C.Receiver.NormalizedName.ToLower().Contains(Specs.Search)));


            AddInclude(C => C.Include(C => C.UserConversationStatuses.Where(C => C.UserId == UserId)));


            if (Specs.Sort == "dsec")
                OrderByDesc = (C => C.Messages.Where(M => M.SenderId == UserId && !M.SenderIsDeleted).Max(M => M.SendAt));

            else
                OrderBy = (C => C.Messages.Where(M => M.SenderId == UserId && !M.SenderIsDeleted).Max(M => M.SendAt));


            ApplyPagination(Specs.PageSize * (Specs.PageNumber - 1), Specs.PageSize);




        }  
    }
}
