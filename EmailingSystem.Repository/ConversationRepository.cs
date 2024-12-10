using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        private readonly EmailDbContext dbContext;

        public ConversationRepository(EmailDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Conversation> GetConversationsByTypeAsync(int UserId, string Type = "inbox")
        {

            //return Type.ToLower() switch
            //{
            //    //"inbox" => dbContext.Conversations.Where(C => C.UserInboxes.Select(C => C.UserId).Contains(C.ReceiverId) && C.ReceiverId == UserId),
            //    "inbox" => await dbContext.UserInboxes.Where(U => U.UserId == UserId).Include(U => U.Conversation).Select(C => C.Conversation).ToListAsync(),
            //    "sent" =>  await dbContext.UserSents.Where(U => U.UserId == UserId).Include(U => U.Conversation).Select(C => C.Conversation).ToListAsync(),
            //    //"trash" => await dbContext.UserConversationStatuses.Where(U => U.UserId == UserId)
            //    _ => throw new ArgumentException("Invalid mail type")
            //};
            return dbContext.Set<Conversation>();
        }

    }
}
