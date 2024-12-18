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

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.PaginatedConversation
{
    public class ConversationSentSpecificationsForCountPagination : BaseSpecification<Conversation>
    {
        public ConversationSentSpecificationsForCountPagination(ConversationSpecParams Specs, int UserId)
        {


            Criteria = C => ((C.ReceiverId == UserId || C.SenderId == UserId) && C.Messages.Any(M => M.SenderId == UserId))
                   &&
                    //(C.UserConversationStatuses
                    //        .Any(C => C.Status == ConversationStatus.Starred || C.Status == ConversationStatus.Active))

                   (C.UserConversationStatuses
                   .Any(C => C.UserId == UserId && (C.Status == ConversationStatus.Starred || C.Status == ConversationStatus.Active)))
                   &&
                   (string.IsNullOrEmpty(Specs.Search) ||
                   (C.Subject.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)
                   ||
                   C.SenderId == UserId || C.Sender.NormalizedName.Contains(Specs.Search,StringComparison.OrdinalIgnoreCase)
                   ||
                   C.ReceiverId == UserId || C.Receiver.NormalizedName.Contains(Specs.Search,StringComparison.OrdinalIgnoreCase)));

        }
    }
}
