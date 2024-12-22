using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs
{
    public class ConversationInboxSpecifications : BaseSpecification<Conversation>
    {
        public ConversationInboxSpecifications(ConversationSpecParams Specs, int UserId)
        {
            Criteria = C => ((C.ReceiverId == UserId || C.SenderId==UserId) && C.Messages.Any(M => M.ReceiverId==UserId))
            &&
            (C.UserConversationStatuses
                    .Any(C => C.UserId == UserId && (C.Status == ConversationStatus.Starred || C.Status == ConversationStatus.Active)))
            &&
            (string.IsNullOrEmpty(Specs.Search) || 
            (C.Subject.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)
            ||
            C.SenderId == UserId || C.Sender.NormalizedName.Contains(Specs.Search,StringComparison.OrdinalIgnoreCase)
            ||
            C.ReceiverId == UserId || C.Receiver.NormalizedName.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)));

            AddInclude(C => C.Include(C => C.UserConversationStatuses.Where(C => C.UserId == UserId)));
          
            AddInclude(C => C.Include(C => C.Messages.Where(M => !M.IsDraft ||( M.IsDraft && M.SenderId==UserId))));


            if (Specs.Sort == "dsec")
                OrderByDesc = (C => C.Messages.Where(M=>M.ReceiverId == UserId && !M.ReceiverIsDeleted && !M.IsDraft).Max(M => M.SendAt));
                
            else
                OrderBy = (C => C.Messages.Where(M => M.ReceiverId == UserId && !M.ReceiverIsDeleted && !M.IsDraft).Max(M => M.SendAt));

            IsPaginated = true;

            ApplyPagination(Specs.PageSize * (Specs.PageNumber - 1), Specs.PageSize);
        }
    }
}
