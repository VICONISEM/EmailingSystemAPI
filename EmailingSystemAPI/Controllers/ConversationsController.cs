using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationRepository conversationRepository;

        public ConversationsController(IConversationRepository conversationRepository)
        {
            this.conversationRepository = conversationRepository;
        }

        int UserId = 1;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> AllConversations(string Type)
        {
            var Conversations = await conversationRepository.GetConversationsByTypeAsync(UserId, Type).ToListAsync();

            return Conversations;
        }
    }
}
