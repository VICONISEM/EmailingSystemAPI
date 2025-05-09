﻿using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.MessagesInConversation;
using EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.PaginatedConversation;
using EmailingSystem.Core.Contracts.Specifications.Contracts.SpecsParams;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystem.Services;
using EmailingSystemAPI.DTOs.Conversation;
using EmailingSystemAPI.DTOs.DraftConversation;
using EmailingSystemAPI.DTOs.Message;
using EmailingSystemAPI.DTOs.User;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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


        [HttpGet("ValidUsersToSend")]
        public async Task<ActionResult<List<AllowedUserDto>>> GetAllowedUsers()
        {
            var Email =  User.FindFirstValue(ClaimTypes.Email);
            if (Email is null)
            {
                return BadRequest();
            }
            var user = await userManager.FindByEmailAsync(Email);
            if (user is null)
            { 
                return BadRequest();
            }
            var Users = await userManager.Users.ToListAsync();


            List<ApplicationUser> ValidUsers = new List<ApplicationUser>();


            foreach (var User in Users)
            {
                if (await userManager.IsUserValidToRecieve(user, User))
                {
                    ValidUsers.Add(User);
                }

            }
            var ValidUsersToSent = mapper.Map<List<AllowedUserDto>>(ValidUsers);

            return Ok(ValidUsersToSent);

        }



        [HttpGet("AllConversations")]
        public async Task<ActionResult<Pagination<ConversationDto>>> AllConversations([FromQuery] ConversationSpecParams Specs)
        {

            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email is null) return NotFound(new APIErrorResponse(404, "User Not Found."));

            var user = await userManager.FindByEmailAsync(Email);
            if (user is null) return NotFound(new APIErrorResponse(404, "User Not Found."));

            IQueryable<Conversation> Query;
            int Count = 0;

            if (Specs.Type == "inbox")
            {
                var specs = new ConversationInboxSpecifications(Specs, user.Id);
                Query =  unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(specs);

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
        public async Task<ActionResult<Pagination<DraftConversationDtoReturn>>> DraftConversations([FromQuery] ConversationSpecParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if(Email is null) return NotFound(new APIErrorResponse(404,"User Not Found."));
            
            var user = await userManager.FindByEmailAsync(Email);
            if(user is null) return NotFound(new APIErrorResponse(404, "User Not Found."));

            var specs = new DraftSpecification(Specs, user.Id);
            var conversations = await unitOfWork.Repository<DraftConversations>().GetAllQueryableWithSpecs(specs).ToListAsync();

            var SpecsCount = new ConversationDraftSpecificationForCountPagination(Specs, user.Id);

            var ConversationCount = await unitOfWork.Repository<DraftConversations>().GetCountWithSpecs(SpecsCount);

            var DraftDto = mapper.Map<List<DraftConversationDtoReturn>>(conversations);

            return Ok(new Pagination<DraftConversationDtoReturn>(Specs.PageNumber,Specs.PageSize,ConversationCount, DraftDto));
        }

        [HttpGet("GetConversationById")]
        public async Task<ActionResult<ConversationToReturnDto>> GetConversationById([FromQuery] ConversationWithMessagesSpecsParams SpecsParams)
        {
            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(SpecsParams.ConversationId);



            if(Conversation == null) return NotFound(new APIErrorResponse(400,"Not Found"));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(userEmail);

            if (user.Id != Conversation.SenderId && user.Id != Conversation.ReceiverId ||Conversation.UserConversationStatuses.Where(US => US.UserId==user.Id).FirstOrDefault().Status==ConversationStatus.Deleted)
                return Unauthorized(new APIErrorResponse(401, "You aren't authorized"));

            var ConversationWithMessages = mapper.Map<ConversationToReturnDto>(Conversation);

            var MessagesSpecs = new MessagesInConversationSpecifications(SpecsParams, user.Id);
            var messages = (await unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(MessagesSpecs).FirstOrDefaultAsync())?.Messages;
            await unitOfWork.Repository<Message>().UpdateRange(M => M.SetProperty(m => m.IsRead, m => m.IsRead||(user.Id==m.ReceiverId) ));
            await unitOfWork.CompleteAsync();
            ConversationWithMessages.Messages=mapper.Map<List<MessageDto>>(messages).OrderByDescending(M=>M.SentAt);

            var DraftMessageSpecs = new GetDraftMessageSpecification( user.Id , SpecsParams.ConversationId);
            var DraftMessage = await unitOfWork.Repository<Conversation>().GetAllQueryableWithSpecs(DraftMessageSpecs).FirstOrDefaultAsync();
            ConversationWithMessages.DraftMessage = mapper.Map<MessageDto>(DraftMessage.Messages.FirstOrDefault());
            return Ok(ConversationWithMessages);
        }

        [HttpPost("ChangeConversationStatus/{Id}/{Status}")]
        public async Task<ActionResult> ChangeConversationStatus(long Id, string Status)
        {
            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(Id);
            if (Conversation is null) return NotFound(new APIErrorResponse(404, "Not Found."));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(userEmail);

            if (user.Id != Conversation.SenderId && user.Id != Conversation.ReceiverId)
                return Unauthorized(new APIErrorResponse(401, "You aren't authorized"));

            var UserConversationStatus = Conversation.UserConversationStatuses.Where(S => S.UserId == user.Id).FirstOrDefault();
            
            if (UserConversationStatus?.Status == ConversationStatus.Deleted)
                return NotFound(new APIErrorResponse(404, "Not Found."));

            if (Enum.TryParse(typeof(ConversationStatus), Status, true, out object? result))
            {
                UserConversationStatus.Status = (ConversationStatus)result;
                unitOfWork.Repository<UserConversationStatus>().Update(UserConversationStatus);
                await unitOfWork.CompleteAsync();
            }
            else
              return BadRequest(new APIErrorResponse(400));

            return Ok();

        }

        [HttpPost("Compose")]
        public async Task<ActionResult> ComposeConversation([FromForm]ConversationComposeDto conversationDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(userEmail);
            var Reciver = await userManager.FindByIdAsync(conversationDto.ReceiverId.ToString());

            if(! await userManager.IsUserValidToRecieve(user,Reciver))
            {
                return BadRequest();
            }

            DraftConversations? draftConversation=null;
            if (conversationDto.Id is not null)
            {
                 draftConversation = await unitOfWork.Repository<DraftConversations>().GetByIdAsync(conversationDto.Id);
                if (draftConversation is null)
                    return NotFound("The draft Conversation Dose'nt Exsite");

            }


            var Conversation = new Conversation()
            {
                SenderId = user.Id,
                ReceiverId = conversationDto.ReceiverId,
                Subject = conversationDto.Subject,
                UserConversationStatuses = new List<UserConversationStatus>() {
                    new UserConversationStatus()
                    { 
                        UserId=user.Id
                        ,Status=ConversationStatus.Active,LastUpdated=DateTime.UtcNow
                    }
                ,   
                    new UserConversationStatus()
                    { 
                    UserId = conversationDto.ReceiverId
                    ,Status = ConversationStatus.Active, LastUpdated = DateTime.UtcNow 
                    }
                }
                ,
                Messages = new List<Message>() { } 
                
            };

            var Message = new Message()
            {
                Attachments= new List<Attachment>(),
                Content= conversationDto.Message.Content,
                ReceiverId= conversationDto.ReceiverId,
                SenderId=user.Id,
            };

            var Attachments = new List<Attachment>() { };

            if(!conversationDto.Message.Attachments.IsNullOrEmpty() || (draftConversation is not null && !draftConversation.DraftAttachments.IsNullOrEmpty() ))
            {
                foreach (var Attachment in conversationDto.Message.Attachments)
                {
                    Attachments.Add(new Attachment()
                    {
                        FileName=Attachment.FileName,
                        FilePath=await FileHandler.SaveFile(Attachment.FileName, "MessageAttachment", Attachment),
                        Size=Attachment.Length
                        
                        
                    });
                }


                if(draftConversation is not null)
                {
                    foreach (var Attachment in draftConversation.DraftAttachments)
                    {
                        Attachments.Add(new Attachment()
                        {
                            FileName = Attachment.Name,
                            FilePath = Attachment.AttachmentPath,
                            Size = Attachment.size


                        });
                    }
                }
                

            }
            Message.Attachments = Attachments;
            Conversation.Messages.Add(Message);

            await unitOfWork.Repository<Conversation>().AddAsync(Conversation);


            if (draftConversation is not null)
            unitOfWork.Repository<DraftConversations>().Delete(draftConversation);
            await unitOfWork.CompleteAsync();

            return Ok("Conversation Added Successfully");

        }

        [HttpPost("ComposeDraft")]
        public async Task<ActionResult> ComposeDraft([FromForm] DraftComposeDto draftComposeDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(userEmail);

            var DraftConversation = new DraftConversations()
            {
                Body = draftComposeDto?.Body,
                DraftAttachments = new List<DraftAttachments>(),
                SenderId = user.Id,
                ReceiverId = draftComposeDto.ReceiverId,
                Subject=draftComposeDto?.Subject,
            };
            if(!draftComposeDto.DraftAttachments.IsNullOrEmpty())
            foreach(var Attachment in draftComposeDto.DraftAttachments)
            {
                    DraftConversation.DraftAttachments.Add(new DraftAttachments()
                    {
                        AttachmentPath = await FileHandler.SaveFile(Attachment.FileName, "DraftConversationAttachment", Attachment),
                        Name= Attachment.FileName,
                        size=Attachment.Length
                        
                    });

            }
            await unitOfWork.Repository<DraftConversations>().AddAsync(DraftConversation);
            await unitOfWork.CompleteAsync();
            var DraftDto = mapper.Map<DraftConversationDtoReturn>(DraftConversation);

            return Ok("Draft Conversation Created");

        }
    
    }
}
