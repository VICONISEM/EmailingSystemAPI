using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystem.Repository;
using EmailingSystemAPI.DTOs;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ConversationsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
        }


        [HttpGet("AllConversations")]
        public async Task<ActionResult<IEnumerable<ConversationDto>>> AllConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);

            IQueryable<Conversation> Query;

            if (Specs.Type == "inbox")
            {
                var specs = new InboxSentSpecifications<UserInbox>(Specs, user.Id);
                Query = unitOfWork.Repository<UserInbox>()
                    .GetAllQueryableWithSpecs(specs)
                    .Select(C => C.Conversation)
                    .Where(C => C.UserConversationStatuses
                    .All(C => C.Status == ConversationStatus.Starred || C.Status == ConversationStatus.Active));
            }
            else if (Specs.Type == "sent")
            {
                var specs = new InboxSentSpecifications<UserSent>(Specs, user.Id);
                Query = unitOfWork.Repository<UserSent>()
                    .GetAllQueryableWithSpecs(specs)
                    .Select(C => C.Conversation)
                    .Where(C => C.UserConversationStatuses
                    .Any(C => C.Status == ConversationStatus.Starred || C.Status == ConversationStatus.Active));
            }
            else
            {
                var specs = new ConversationSpecifications(Specs,user.Id);
                Query = unitOfWork.Repository<Conversation>()
                    .GetAllQueryableWithSpecs(specs)
                    .Where(C => C.UserConversationStatuses
                    .All(C => C.Status == (ConversationStatus)Enum.Parse(typeof(ConversationStatus),Specs.Type)));
            }

            var conversations = await Query.ToListAsync();

            var ConversationDto = mapper.Map<IEnumerable<ConversationDto>>(conversations);

            return Ok(ConversationDto);
        }

        [HttpGet("DraftConversations")]
        public async Task<ActionResult<IEnumerable<DraftConversations>>> DraftConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);

            var specs = new DraftSpecification(Specs, user.Id);
            var conversations = unitOfWork.Repository<DraftConversations>().GetAllQueryableWithSpecs(specs);

            return await conversations.ToListAsync();
        }

        [HttpPost("")]



    }
}
