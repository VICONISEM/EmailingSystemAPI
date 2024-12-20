using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("{ConversationId}")]
        public async Task<ActionResult> SendMessage([FromBody]MessageTobeSentDto messageTobeSentDto,[FromRoute]long ConversationId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = userManager.FindByIdAsync(userEmail);

            var Conversation = await unitOfWork.Repository<Conversation>().GetByIdAsync<long>(ConversationId);
            if(Conversation is null)
            {
                return NotFound();
            }

            if(messageTobeSentDto.Content.IsNullOrEmpty() && messageTobeSentDto.Attachements.IsNullOrEmpty())
            {
                return BadRequest("Cant Create Empty Message");
            }

            var Message = new Message()
            {
                Content = messageTobeSentDto?.Content,
                Attachments = new List<Attachment>() { },
                ParentMessageId = messageTobeSentDto?.ParentMessage,
                SenderId = user.Id,
                ReceiverId = (Conversation.SenderId==user.Id) ? Conversation.ReceiverId: Conversation.SenderId,

            };

            foreach(var Attachment  in messageTobeSentDto.Attachements)
            {
                Message.Attachments.Add(new Attachment()
                {
                    FileName=Attachment.FileName,
                    FilePath="",
       
                });
            }

            await unitOfWork.Repository<Message>().AddAsync(Message);
            return Ok("Message Send Successfully");


        }


    }
}
