using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.MessagesInConversation;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.PaginatedConversation;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystemAPI.DTOs;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Helper;
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
        private readonly IMapper mapper;

        public ConversationsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
        }


        [HttpGet("AllConversations")]
        public async Task<ActionResult<Pagination<ConversationDto>>> AllConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);

            IQueryable<Conversation> Query;
            int Count = 0;

            if (Specs.Type == "inbox")
            {
                var specs = new ConversationInboxSpecifications(Specs, user.Id);
                Query = unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(specs);

                var CountSpecs = new ConversationInboxSpecificationsForCountPagination(Specs, user.Id);
                Count = await unitOfWork.Repository<Conversation>().GetCountWithSpecs(CountSpecs);
            }
            else if (Specs.Type == "sent")
            {
                var specs = new ConversationSentSpecifications(Specs, user.Id);
                Query = unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(specs);

                var CountSpecs = new ConversationSentSpecificationsForCountPagination(Specs, user.Id);
                Count = await unitOfWork.Repository<Conversation>().GetCountWithSpecs(CountSpecs);
            }
            else
            {
                var specs = new ConversationSpecifications(Specs, user.Id);
                Query = unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(specs);

                var CountSpecs = new ConversationSpecificationsForCountPagination(Specs, user.Id);
                Count = await unitOfWork.Repository<Conversation>().GetCountWithSpecs(CountSpecs);
            }

            var conversations = await Query.ToListAsync();

            var ConversationDtoList = mapper.Map<IReadOnlyList<ConversationDto>>(conversations);

            return Ok(new Pagination<ConversationDto>(Specs.PageNumber,Specs.PageSize,Count,ConversationDtoList));
        }

        [HttpGet("DraftConversations")]
        public async Task<ActionResult<Pagination<DraftConversations>>> DraftConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);

            var specs = new DraftSpecification(Specs, user.Id);
            var conversations = await unitOfWork.Repository<DraftConversations>().GetAllQueryableWithSpecs(specs).ToListAsync();

            var SpecsCount = new ConversationDraftSpecificationForCountPagination(Specs, user.Id);
            var ConversationCount = await unitOfWork.Repository<DraftConversations>().GetCountWithSpecs(SpecsCount);

            return Ok(new Pagination<DraftConversations>(Specs.PageNumber,Specs.PageSize,ConversationCount,conversations));
        }

        [HttpGet("GetConversationById")]
        public async Task<ActionResult<ConversationToReturnDto>> GetConversationById([FromQuery] ConversationWithMessagesSpecsParams SpecsParams)
        {
            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(SpecsParams.ConversationId);
            if(Conversation == null) return NotFound(new APIErrorResponse(400,"Not Found"));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = userManager.FindByIdAsync(userEmail);

            if (user.Id != Conversation.SenderId && user.Id != Conversation.ReceiverId)
                return Unauthorized(new APIErrorResponse(401, "You aren't authorized"));

            var ConversationWithMessages = mapper.Map<ConversationToReturnDto>(Conversation);

            var MessagesSpecs = new MessagesInConversationSpecifications(SpecsParams, user.Id);
            var messages = (await unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(MessagesSpecs).FirstOrDefaultAsync())?.Messages;
            ConversationWithMessages.Messages=mapper.Map<List<MessageDto>>(messages);

            var DraftMessageSpecs = new GetDraftMessageSpecification( user.Id , SpecsParams.ConversationId);
            var DraftMessage = (await unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(DraftMessageSpecs).FirstOrDefaultAsync())?.Messages;
            ConversationWithMessages.DraftMessage = mapper.Map<MessageDto>(DraftMessage);
            return Ok(ConversationWithMessages);
        }

        [HttpPost("ChangeConversationStatus/{Id}/{Status}")]
        public async Task<ActionResult> ChangeConversationStatus(long Id, string Status)
        {
            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(Id);
            if (Conversation is null) return NotFound(new APIErrorResponse(404, "Not Found."));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = userManager.FindByIdAsync(userEmail);

            if (user.Id != Conversation.SenderId && user.Id != Conversation.ReceiverId)
                return Unauthorized(new APIErrorResponse(401, "You aren't authorized"));

            var UserConversationStatus = Conversation.UserConversationStatuses.Where(S => S.UserId == user.Id).FirstOrDefault();
            
            if (UserConversationStatus?.Status == ConversationStatus.Deleted)
                return NotFound(new APIErrorResponse(404, "Not Found."));

            if (Enum.TryParse(typeof(ConversationStatus), Status, true, out object? result))
            {
                UserConversationStatus.Status = (ConversationStatus)result;
                unitOfWork.Repository<UserConversationStatus>().Update(UserConversationStatus);
            }
            else
              return BadRequest(new APIErrorResponse(400));

            return Ok();

        }
    
        
    
    }
}
