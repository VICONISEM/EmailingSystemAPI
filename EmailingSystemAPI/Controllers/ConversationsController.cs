using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using EmailingSystem.Repository;
using EmailingSystem.Repository.Data.Contexts;
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
        private readonly EmailDbContext context;

        public ConversationsController(IConversationRepository conversationRepository,EmailDbContext context)
        {
            this.conversationRepository = conversationRepository;
            this.context = context;
        }

        int UserId = 1;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> AllConversations(string Type)
        {
            var Conversations = await conversationRepository.GetConversationsByTypeAsync(UserId, Type).ToListAsync();

            return Conversations;
        }

        [HttpGet("{AllConversationsWithSpecs}")]
        public async Task<ActionResult<IEnumerable<Conversation>>> AllConversationsWithSpecs(string Type)
        {
            var BaseSpec = new BaseSpecification<UserInbox>(x => x.UserId == UserId) {};
            BaseSpec.Includes.Add(u => u.Conversation);
            var Conversations = await SpecificationEvalutor<UserInbox>.GetQuery(context.Set<UserInbox>(),BaseSpec).Select(x=>x.Conversation).ToListAsync();

            return Conversations;
        }
    }
}
