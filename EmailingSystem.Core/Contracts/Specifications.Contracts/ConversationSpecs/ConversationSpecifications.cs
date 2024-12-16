using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;

public class ConversationSpecifications : BaseSpecification<Conversation>
{
    public ConversationSpecifications(ConversationSpecParams Specs, int UserId)
    {
        Criteria = C => ((C.ReceiverId == UserId || C.SenderId == UserId))
               &&
               (C.UserConversationStatuses
               .Any(C => C.Status == (ConversationStatus)Enum.Parse(typeof(ConversationStatus), Specs.Type))
               &&
               (string.IsNullOrEmpty(Specs.Search) ||
               (C.Subject.ToUpper().Contains(Specs.Search)
               ||
               C.SenderId == UserId || C.Sender.NormalizedName.Contains(Specs.Search)
               ||
               C.ReceiverId == UserId || C.Receiver.NormalizedName.Contains(Specs.Search))));

             AddInclude(C => C.Include(C => C.UserConversationStatuses.Where(C => C.UserId == UserId)));

        if (Specs.Sort == "dsec")
            OrderByDesc = (C => C.Messages.Where(C => C.SenderId == UserId && !C.SenderIsDeleted || C.ReceiverId == UserId && !C.ReceiverIsDeleted).Max(M => M.SendAt));

        else
            OrderBy = (C => C.Messages.Where(C => C.SenderId == UserId && !C.SenderIsDeleted || C.ReceiverId == UserId && !C.ReceiverIsDeleted).Max(M => M.SendAt));


        ApplyPagination(Specs.PageSize * (Specs.PageNumber - 1), Specs.PageSize);
    }
}