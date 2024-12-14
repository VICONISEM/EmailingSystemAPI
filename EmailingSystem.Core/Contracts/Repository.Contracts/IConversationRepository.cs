using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Repository.Contracts
{
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        public IQueryable<Conversation> GetInboxOrSentAsync(int UserId, string Type);
    }
}
