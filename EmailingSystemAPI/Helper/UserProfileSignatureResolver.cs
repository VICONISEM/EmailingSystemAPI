using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class UserProfileSignatureResolver : IValueResolver<ApplicationUser, UserDto, string>
    {
        private readonly IHttpContextAccessor configuration;

        public UserProfileSignatureResolver(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(ApplicationUser source, UserDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Signature?.FilePath))
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.Signature.FilePath}";
            }
            else
            {
                return "Empty";
            }
        }

    }
}
