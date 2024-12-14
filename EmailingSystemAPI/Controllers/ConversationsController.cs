using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts.Specification.Contract;
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



        [HttpGet("AllConversationsWithSpecs")]
        public async Task<ActionResult<IEnumerable<UserInbox>>> AllConversationsWithSpecs([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);


            
            var specs = new InboxSpecifications(Specs,user.Id);
            var conversations = unitOfWork.Repository<UserInbox>().GetAllQueryableWithSpecs(specs);


            return await conversations.ToListAsync();
        }


    }
}
