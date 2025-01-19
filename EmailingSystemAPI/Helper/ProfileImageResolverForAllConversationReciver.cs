using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Conversation;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class ProfileImageResolverForAllConversationReciver : IValueResolver<Conversation, ConversationDto, string>
    {

        private readonly IHttpContextAccessor configuration;

        public ProfileImageResolverForAllConversationReciver(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(Conversation source, ConversationDto destination, string destMember, ResolutionContext context)
        {
            if (source.Receiver.PicturePath is not null && destination.ReceiverPictureURL is null)
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









