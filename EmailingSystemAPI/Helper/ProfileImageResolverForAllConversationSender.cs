using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Conversation;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class ProfileImageResolverForAllConversationSender : IValueResolver<Conversation, ConversationDto, string>
    {

        private readonly IHttpContextAccessor configuration;

        public ProfileImageResolverForAllConversationSender(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(Conversation source, ConversationDto destination, string destMember, ResolutionContext context)
        {
            if (source.Sender.PicturePath is not null && destination.SenderPictureURL is null)
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.Sender.PicturePath}";
            }
            else
            {
                return "Empty";
            }


        }

    }
}









