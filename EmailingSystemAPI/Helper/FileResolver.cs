using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.Attachement;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class FileResolver:IValueResolver<DraftAttachments,AttachementDto,string>
    {

        private readonly IHttpContextAccessor configuration;

        public FileResolver(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(DraftAttachments source, AttachementDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.AttachmentPath))
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.AttachmentPath}";
            }
            else
            {
                return "Empty";
            }
        }


    }
}
