using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.SpecsParams;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationRepository conversationRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ConversationsController(IConversationRepository conversationRepository, UserManager<ApplicationUser> userManager)
        {
            this.conversationRepository = conversationRepository;
            this.userManager = userManager;
      
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> InboxOrSendConversations([FromQuery] ConversationSpecParams Specs)
        {
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(UserEmail);

            //var Conversations = await conversationRepository.GetConversationsByTypeAsync(user.Id, Specs.Type)
            //                                           .Where(C =>
            //                                           (string.IsNullOrEmpty(Specs.Search) || C.Subject.ToLower().Contains(Specs.Search))
            //                                           ||
            //                                           (string.IsNullOrEmpty(Specs.Search) || C.SenderId == user.Id || C.Sender.Name.ToLower().Contains(Specs.Search))
            //                                           ||
            //                                           (string.IsNullOrEmpty(Specs.Search) || C.ReceiverId == user.Id || C.Receiver.Name.ToLower().Contains(Specs.Search))).ToListAsync();


            var Query = conversationRepository.GetInboxOrSentAsync(user.Id, Specs.Type);
            var Conversations = await InboxAndSentQueryBuilder.Build(Query,Specs,user.Id).ToListAsync(); 

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
