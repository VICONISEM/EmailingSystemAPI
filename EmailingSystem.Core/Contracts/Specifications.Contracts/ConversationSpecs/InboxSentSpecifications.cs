using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams
{
    public class InboxSentSpecifications<T> : BaseSpecification<T> where T : BaseInboxSent
    {
        //public static IQueryable<Conversation> Build(IQueryable<Conversation> Query, ConversationSpecParams Specs, int UserId)
        //{
        //    if (!string.IsNullOrEmpty(Specs.Search))
        //    {
        //        Query.Where(C =>
        //             C.Subject.ToLower().Contains(Specs.Search)
        //             |
        //             (C.SenderId == UserId || C.Sender.NormalizedName.ToLower().Contains(Specs.Search))
        //             |
        //             (C.ReceiverId == UserId || C.Receiver.NormalizedName.ToLower().Contains(Specs.Search)));
        //    }

        //    if (Specs.Sort is not null)
        //    {
        //        if (Specs.Sort == "dsec")
        //            Query.OrderByDescending(C => C.LastMessage.SendAt);
        //        else
        //            Query.OrderBy(C => C.LastMessage.SendAt);
        //    }

        //    Query.Skip(Specs.PageSize * (Specs.PageNumber - 1)).Take(Specs.PageSize);

        //    return Query;
        //}

        public InboxSentSpecifications(ConversationSpecParams Specs, int UserId) : base()
        {

            Criteria = C => C.UserId == UserId && 
            string.IsNullOrEmpty(Specs.Search) || C.Conversation.Subject.ToLower().Contains(Specs.Search)
            ||
            (C.Conversation.SenderId == UserId || C.Conversation.Sender.NormalizedName.ToLower().Contains(Specs.Search))
            ||
            (C.Conversation.ReceiverId == UserId || C.Conversation.Sender.NormalizedName.ToLower().Contains(Specs.Search));

            AddInclude(Ui => Ui.Include(C => C.Conversation).ThenInclude(C => C.UserConversationStatuses));






            if (Specs.Sort == "dsec")
                OrderByDesc = C => C.Conversation.Messages.Where(C => C.ReceiverId == UserId && !C.ReceiverIsDeleted).Max(M => M.SendAt);
            else
                OrderBy = C => C.Conversation.Messages.Where(C => C.SenderId == UserId && !C.SenderIsDeleted || C.ReceiverId == UserId && !C.ReceiverIsDeleted).Max(M => M.SendAt);


            ApplyPagination(Specs.PageSize * (Specs.PageNumber - 1), Specs.PageSize);
        }

    }
}
