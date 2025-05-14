using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Services;
using EmailingSystemAPI.DTOs;
using EmailingSystemAPI.DTOs.Message;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<Notification> hubContext;

        public MessageController(IUnitOfWork unitOfWork,IMapper mapper,UserManager<ApplicationUser> userManager,IHubContext<Notification> hub)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
            this.hubContext = hub;
        }

        [HttpPost("SendMessage/{conversationId}")]
        public async Task<ActionResult> SendMessage([FromForm]MessageTobeSentDto messageTobeSentDto,[FromRoute]long conversationId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(userEmail);

            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(conversationId);

            if (Conversation is null) return NotFound(new APIErrorResponse(404, "Conversation Not Found."));

            if(Conversation.SenderId != user.Id && Conversation.ReceiverId != user.Id)
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to send message in this conversation."));
            }

            Message ParentMessage = null;

            if (messageTobeSentDto.ParentMessageId.HasValue)
            {
                ParentMessage = await unitOfWork.Repository<Message>().GetByIdAsync(messageTobeSentDto.ParentMessageId);
                if (ParentMessage is null) return BadRequest(new APIErrorResponse(400, "Message Not Found."));
            }

            if (messageTobeSentDto.Content.IsNullOrEmpty() && messageTobeSentDto.Attachements.IsNullOrEmpty())
            {
                return BadRequest(new APIErrorResponse(400, "Can't create empty message."));
            }

            var Message = new Message()
            {
                Content = messageTobeSentDto?.Content,
                Attachments = new List<Attachment>() { },
                ParentMessageId = messageTobeSentDto?.ParentMessageId,
                ConversationId = conversationId,
                SenderId = user.Id,
                ReceiverId = (Conversation.SenderId == user.Id) ? Conversation.ReceiverId : Conversation.SenderId,
                IsDraft = false
            };

            foreach(var Attachment  in messageTobeSentDto.Attachements)
            {
                Message.Attachments.Add(new Attachment()
                {
                    FileName = Attachment.FileName,
                    FilePath = await FileHandler.SaveFile(Attachment.FileName,"MessageAttachment",Attachment),
                    Size=Attachment.Length
                });
            }

            Conversation.Messages.Add(Message);
            unitOfWork.Repository<Conversation>().Update(Conversation);
            await unitOfWork.CompleteAsync();
            var emailre = (Conversation.Receiver.Email == user.Email) ? Conversation.Sender.Email : Conversation.Receiver.Email;
            await hubContext.Clients.User(emailre.ToUpper()).SendAsync("Notification", new { Note = $"You get Message From {user.Email}"});
            await hubContext.Clients.User(emailre.ToUpper()).SendAsync("MessageInsideConversation", mapper.Map<MessageDto>(Message));

            return Ok("Message Send Successfully");
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
                Message.Content = "this Message is Deleted";
                foreach(var attachment in Message.Attachments)
                {
                    await FileHandler.DeleteFile(attachment.FilePath);
                }
            }
            else
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to delete this message."));

            }

            unitOfWork.Repository<Message>().Update(Message);
            await unitOfWork.CompleteAsync();

            return Ok("Message Deleted");
        }

        [HttpPost("SaveDraftMessage/{ConversationId}")]
        public async Task<ActionResult> SaveDraftMessage([FromForm] MessageTobeSentDto messageDto, [FromRoute] long ConversationId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(ConversationId);

            if (Conversation is null) return NotFound(new APIErrorResponse(404, "Conversation Not Found."));

            if (Conversation.SenderId != user.Id && Conversation.ReceiverId != user.Id)
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to send message in this conversation."));
            }

            if (Conversation.Messages.Any(x => x.IsDraft == true && x.SenderId == user.Id && x.ReceiverId == (Conversation.SenderId == user.Id ? Conversation.ReceiverId : Conversation.SenderId)))
            {
                return BadRequest(new APIErrorResponse(400, "You already have a draft message in this conversation."));
            }

            Message ParentMessage = null;

            if (messageDto.ParentMessageId.HasValue)
            {
                ParentMessage = await unitOfWork.Repository<Message>().GetByIdAsync(messageDto.ParentMessageId);
                if (ParentMessage is null) return BadRequest(new APIErrorResponse(400, "Message Not Found."));

            }

            if (messageDto.Content.IsNullOrEmpty() && messageDto.Attachements.IsNullOrEmpty())
            {
                return BadRequest(new APIErrorResponse(400, "Can't create empty message."));
            }

            var Message = new Message()
            {
                SenderId = user.Id,
                Content = messageDto.Content,
                ReceiverId = (user.Id == Conversation.SenderId ? Conversation.ReceiverId : Conversation.SenderId),
                ParentMessageId = messageDto.ParentMessageId,
                Attachments = new List<Attachment>() { },
                IsDraft = true,
                ConversationId = ConversationId
            };

            foreach (var Attachment in messageDto.Attachements)
            {
                Message.Attachments.Add(new Attachment()
                {
                    FileName = Attachment.FileName,
                    FilePath = await FileHandler.SaveFile(Attachment.FileName, "DraftMessageAttachment", Attachment),
                });
            }

            await unitOfWork.Repository<Message>().AddAsync(Message);
            await unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("SendDraftMessage/{MessageId}")]
        public async Task<ActionResult> SendDraftMessage([FromRoute] long MessageId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            var Message = await unitOfWork.Repository<Message>().GetByIdAsync<long>(MessageId);

            if (Message is null) return NotFound(new APIErrorResponse(404, "Message Not Found."));

            if (Message.SenderId != user.Id)
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to send this message."));
            }

            if (Message.IsDraft == false)
            {
                return BadRequest(new APIErrorResponse(400, "This message is not a draft."));
            }

            Message.IsDraft = false;

            unitOfWork.Repository<Message>().Update(Message);
            await unitOfWork.CompleteAsync();

            return Ok("Draft Message Send Successfully");
        }

        [HttpPost("UpdateDraftMessage/{MessageId}")]
        public async Task<ActionResult> UpdateDraftMessage([FromForm] MessageTobeSentDto messageDto, [FromRoute] long MessageId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            var Message = await unitOfWork.Repository<Message>().GetByIdAsync<long>(MessageId);

            if (Message is null) return NotFound(new APIErrorResponse(404, "Message Not Found."));

            if (Message.SenderId != user.Id)
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to update this message."));
            }

            if (Message.IsDraft == false)
            {
                return BadRequest(new APIErrorResponse(400, "This message is not a draft."));
            }

            if (messageDto.Content.IsNullOrEmpty() && messageDto.Attachements.IsNullOrEmpty())
            {
                return BadRequest(new APIErrorResponse(400, "Can't create empty message."));
            }

            Message.Content = messageDto.Content;

            foreach (var Attachment in messageDto.Attachements)
            {
                Message.Attachments.Add(new Attachment()
                {
                    FileName = Attachment.FileName,
                    FilePath = await FileHandler.SaveFile(Attachment.FileName, "DraftMessageAttachment", Attachment),
                });
            }

            unitOfWork.Repository<Message>().Update(Message);
            await unitOfWork.CompleteAsync();

            return Ok("Draft Message Updated Successfully");
        }

        [HttpPost("DeleteAttachementOfDraftMessage/{AttachementId}")]
        public async Task<ActionResult> DeleteAttachementOfDraftMessage(int AttachementId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            var Attachment = await unitOfWork.Repository<Attachment>().GetByIdAsync<int>(AttachementId);

            if(Attachment is null)
            {                 
                return NotFound(new APIErrorResponse(404, "Attachment Not Found."));
            }

            var Message = Attachment.Message;

            if (Message.SenderId != user.Id)
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to delete this Attachment."));
            }

            if(Message.IsDraft == false)
            {
                return BadRequest(new APIErrorResponse(400, "This message is not a draft."));
            }

            if(await FileHandler.DeleteFile(Attachment.FilePath))
            {
                unitOfWork.Repository<Attachment>().Delete(Attachment);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                return BadRequest(new APIErrorResponse(400, "Attachment Not Found."));
            }

            return Ok("Attachment Deleted Successfully");

        }

        [HttpPost("DeleteDraftMessage/{MessageId}")]
        public async Task<ActionResult> DeleteDraftMesage(long MessageId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user  = await userManager.FindByEmailAsync(Email);

            var Message = await unitOfWork.Repository<Message>().GetByIdAsync<long>(MessageId);

            if (Message is null) return NotFound(new APIErrorResponse(404, "Message Not Found."));

            if (Message.SenderId != user.Id)
            {
                return Unauthorized(new APIErrorResponse(401, "You aren't authourized to delete this message."));
            }

            if (Message.IsDraft == false) {
                return BadRequest(new APIErrorResponse(400, "This message is not a draft."));
            }

            foreach (var attachment in Message.Attachments)
            {
                await FileHandler.DeleteFile(attachment.FilePath);
            }
            unitOfWork.Repository<Message>().Delete(Message);
            await unitOfWork.CompleteAsync();
            return Ok("Draft Message Deleted Successully");
        }

    }
}
