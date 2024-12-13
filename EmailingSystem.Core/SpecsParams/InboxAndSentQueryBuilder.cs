using EmailingSystem.Core.Entities;

namespace EmailingSystem.Core.SpecsParams
{
    public static class InboxAndSentQueryBuilder
    {
        public static IQueryable<Conversation> Build(IQueryable<Conversation> Query, ConversationSpecParams Specs, int UserId)
        {
            if(!string.IsNullOrEmpty(Specs.Search))
            {
                Query.Where(C =>
                     (C.Subject.ToLower().Contains(Specs.Search))
                     |
                     (C.SenderId == UserId || C.Sender.Name.ToLower().Contains(Specs.Search))
                     |
                     (C.ReceiverId == UserId || C.Receiver.Name.ToLower().Contains(Specs.Search)));
            }

            if(Specs.Sort is not null)
            {
                if (Specs.Sort == "dsec")
                    Query.OrderByDescending(C => C.LastMessage.SendAt);
                else
                    Query.OrderBy(C => C.LastMessage.SendAt);
            }

            Query.Skip(Specs.PageSize * (Specs.PageNumber - 1) ).Take(Specs.PageSize);

            return Query;
        }
    }
}
