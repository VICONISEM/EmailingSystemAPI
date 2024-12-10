using EmailingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Repository.Contracts
{
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        public IQueryable<Conversation> GetConversationsByTypeAsync(int UserId, string Type);
    }
}
