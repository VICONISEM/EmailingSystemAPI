using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Repository;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public ConversationsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
      
        }



        [HttpGet("AllConversations")]
        public async Task<ActionResult<IEnumerable<UserInbox>>> AllConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);

            if (Specs.Type == "inbox")
            {
                var specs = new InboxSpecifications<UserInbox>(Specs, user.Id);
                var conversations = unitOfWork.Repository<UserInbox>().GetAllQueryableWithSpecs(specs);
            }
            else if (Specs.Type == "sent")
            {
                var specs = new InboxSpecifications<UserSent>(Specs, user.Id);
                var conversations = unitOfWork.Repository<UserSent>().GetAllQueryableWithSpecs(specs);
            }
            else



            return await conversations.ToListAsync();
        }

        [HttpGet("DraftConversations")]
        public async Task<ActionResult<IEnumerable<UserInbox>>> DraftConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);

            var specs = new DraftSpecifications(Specs, user.Id);
            var conversations = unitOfWork.Repository<Draft>().GetAllQueryableWithSpecs(specs);


            return await conversations.ToListAsync();
        }


    }
}
