using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class MessageAttachmentResolver : IValueResolver<Attachment, AttachementDto, string>
    {
        private readonly IHttpContextAccessor configuration;

        public MessageAttachmentResolver(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(Attachment source, AttachementDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.FilePath))
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.FilePath}";
            }
            else
            {
                return "Empty";
            }
        }

    }
}
