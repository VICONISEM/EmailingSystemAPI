using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.PaginatedConversation
{
    public class ConversationSpecificationsForCountPagination : BaseSpecification<Conversation>
    {
        public ConversationSpecificationsForCountPagination(ConversationSpecParams Specs, int UserId)
        {

            Criteria = C => ((C.ReceiverId == UserId || C.SenderId == UserId))
                   &&
                   C.UserConversationStatuses
                   .Any(C => C.Status == (ConversationStatus)Enum.Parse(typeof(ConversationStatus), Specs.Type))
                   &&
                   (string.IsNullOrEmpty(Specs.Search) ||
                   (C.Subject.ToUpper().Contains(Specs.Search)
                   ||
                   C.SenderId == UserId || C.Sender.NormalizedName.Contains(Specs.Search)
                   ||
                   C.ReceiverId == UserId || C.Receiver.NormalizedName.Contains(Specs.Search)));
        }
    }
}
