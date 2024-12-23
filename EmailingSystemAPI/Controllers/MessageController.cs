using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Services;
using EmailingSystemAPI.DTOs.Message;
using EmailingSystemAPI.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public MessageController(IUnitOfWork unitOfWork,IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpPost("SendMessage/{ConversationId}")]
        public async Task<ActionResult> SendMessage([FromBody]MessageTobeSentDto messageTobeSentDto,[FromRoute]long ConversationId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = userManager.FindByIdAsync(userEmail);

            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(ConversationId);

            if (Conversation is null) return NotFound(new APIErrorResponse(404, "Conversation Not Found."));


            if (messageTobeSentDto.Content.IsNullOrEmpty() && messageTobeSentDto.Attachements.IsNullOrEmpty())
            {
                return BadRequest(new APIErrorResponse(400, "Can't create empty message."));
            }

            var Message = new Message()
            {
                Content = messageTobeSentDto?.Content,
                Attachments = new List<Attachment>() { },
                ParentMessageId = messageTobeSentDto?.ParentMessageId,
                ConversationId = ConversationId,
                SenderId = user.Id,
                ReceiverId = (Conversation.SenderId == user.Id) ? Conversation.ReceiverId : Conversation.SenderId,
            };

            foreach(var Attachment  in messageTobeSentDto.Attachements)
            {
                Message.Attachments.Add(new Attachment()
                {
                    FileName = Attachment.FileName,
                    FilePath = await FileHandler.SaveFile(Attachment.FileName,"MessageAttachment",Attachment.File),
                });
            }

            await unitOfWork.Repository<Message>().AddAsync(Message);
            await unitOfWork.CompleteAsync();

            return Ok("Message Send Successfully");
        }

        [HttpPost("SaveDraftMessage/{ConversationId}")]
        public async Task<ActionResult> SaveDraftMessage([FromBody] MessageTobeSentDto messageDto, [FromRoute]long conversationId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(conversationId);
            
            if (Conversation is null) return NotFound(new APIErrorResponse(404,"Conversation Not Found."));
            

            if (messageDto.Content.IsNullOrEmpty() && messageDto.Attachements.IsNullOrEmpty())
            {
                return BadRequest(new APIErrorResponse(400,"Can't create empty message."));
            }

            var Message = new Message()
            {
                SenderId = user.Id,
                Content = messageDto.Content,
                ReceiverId = messageDto.ReceiverId,
                ConversationId = conversationId,
                ParentMessageId = messageDto.ParentMessageId,

                Attachments = new List<Attachment>() { },

                IsDraft = true
            };

            foreach (var Attachment in messageDto.Attachements)
            {
                Message.Attachments.Add(new Attachment()
                {
                    FileName = Attachment.FileName,
                    FilePath = await FileHandler.SaveFile(Attachment.FileName, "DraftMessageAttachment", Attachment.File),
                });
            }

            await unitOfWork.Repository<Message>().AddAsync(Message);
            await unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("DeleteForMe/{MessageId}")]
        public async Task<ActionResult> DeleteForMe([FromRoute]long MessageId)
        {
            var Message = await unitOfWork.Repository<Message>().GetByIdAsync<long>(MessageId);
            if(Message is null) return NotFound(new APIErrorResponse(400, "Message Not Found."));

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            if(Message.SenderId != user.Id && Message.ReceiverId != user.Id)
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to delete this message."));
            }
            else if(Message.SenderId == user.Id)
            {
                Message.SenderIsDeleted = true;
            }
            else
            {
                Message.ReceiverIsDeleted = true;
            }

            unitOfWork.Repository<Message>().Update(Message);
            await unitOfWork.CompleteAsync();

            return Ok("Message Deleted");
        }

        [HttpPost("DeleteForEveryOne/{MessageId}")]
        public async Task<ActionResult> DeleteForEveryOne([FromRoute] long MessageId)
        {
            var Message = await unitOfWork.Repository<Message>().GetByIdAsync<long>(MessageId);
            if (Message is null) return NotFound(new APIErrorResponse(400, "Message Not Found."));

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            if (Message.SenderId == user.Id)
            {
                Message.SenderIsDeleted = true;
                Message.ReceiverIsDeleted = true;
            }
            else
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to delete this message."));

            }

            unitOfWork.Repository<Message>().Update(Message);
            await unitOfWork.CompleteAsync();

            return Ok("Message Deleted");
        }

    }
}
