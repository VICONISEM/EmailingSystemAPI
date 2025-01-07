using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Conversation;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class ProfileImageResolverForAllConversation : IValueResolver<Conversation, ConversationDto, string>
    {

        private readonly IHttpContextAccessor configuration;

        public ProfileImageResolverForAllConversation(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(Conversation source, ConversationDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Sender.PicturePath) && destination.SenderPictureURL is null)
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.Sender.PicturePath}";
            }
            if (!string.IsNullOrEmpty(source.Receiver.PicturePath) && destination.SenderPictureURL is null)
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.Receiver.PicturePath}";
            }

            else
            {
                return "Empty";
            }
        }

    }
}









