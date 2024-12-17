﻿using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.PaginatedConversation;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystem.Repository;
using EmailingSystemAPI.DTOs;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Helper;
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
        public async Task<ActionResult<IEnumerable<DraftConversations>>> DraftConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByIdAsync(Email);

            var specs = new DraftSpecification(Specs, user.Id);
            var conversations = unitOfWork.Repository<DraftConversations>().GetAllQueryableWithSpecs(specs);

            return await conversations.ToListAsync();
        }

        [HttpGet("GetConversationById")]
        public async Task<ActionResult<ConversationToReturnDto>> GetConversationById(int ConversationId)
        {
            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync(ConversationId);
            if(Conversation == null) return NotFound(new APIErrorResponse(400,"Not Found"));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = userManager.FindByIdAsync(userEmail);

            if (user.Id != Conversation.SenderId && user.Id != Conversation.ReceiverId)
                return Unauthorized(new APIErrorResponse(401, "You aren't authorized"));

            var ConversationWithMessages = mapper.Map<ConversationToReturnDto>(Conversation);

            var MessagesSpecs = new MessagesInConversationSpecifications();
            var Messages = await unitOfWork.Repository<Message>().GetAllQueryableWithSpecs(MessagesSpecs);

            for(int i = 0; i < Messages.Count; i++)
            {

            }


            return Ok(ConversationWithMessages);
        }


    }
}
