using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
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

        public IQueryable<Conversation> GetInboxOrSentAsync(int UserId, string Type = "inbox")
        {

            return Type.ToLower() switch
            {
                //"sent" =>  dbContext.UserSents.Where(U => U.UserId == UserId).Include(U => U.Conversation).Select(C => C.Conversation).Where(c=>c.UserConversationStatuses.Any(S => S.Status == ConversationStatus.Active)).Include(C => C.LastMessage),
                //_ => dbContext.UserInboxes.Where(U => U.UserId == UserId).Include(U => U.Conversation).Select(C => C.Conversation).Where(c => c.UserConversationStatuses.Any(S => S.Status == ConversationStatus.Active)).Include(C => C.LastMessage),

                "sent" => dbContext.UserSents.Where(U => U.UserId == UserId).Include(U => U.Conversation).ThenInclude(C => C.LastMessage).Select(C => C.Conversation).Where(c => c.UserConversationStatuses.Any(S => S.Status == ConversationStatus.Active || S.Status == ConversationStatus.Starred)),
                _ => dbContext.UserInboxes.Where(U => U.UserId == UserId).Include(U => U.Conversation).ThenInclude(C => C.LastMessage).Select(C => C.Conversation).Where(c => c.UserConversationStatuses.Any(S => S.Status == ConversationStatus.Active || S.Status == ConversationStatus.Starred)),
            };
        }
    }
}
