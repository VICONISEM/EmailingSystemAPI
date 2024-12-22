using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Conversation;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class ProfileImageResolverForConversation:IValueResolver<Conversation,ConversationToReturnDto, string>
    {

        private readonly IHttpContextAccessor configuration;

        public ProfileImageResolverForConversation(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(Conversation source, ConversationToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Sender.PicturePath))
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.Sender.PicturePath}";
            }
            if (!string.IsNullOrEmpty(source.Receiver.PicturePath))
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









