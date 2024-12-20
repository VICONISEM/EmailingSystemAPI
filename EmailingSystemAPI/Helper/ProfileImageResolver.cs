using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class ProfileImageResolver : IValueResolver<ApplicationUser, AuthDto, string>
    {
        private readonly IHttpContextAccessor configuration;

        public ProfileImageResolver(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(ApplicationUser source, AuthDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PicturePath))
            {
                var request = configuration?.HttpContext?.Request;
                return $"{request?.Scheme}://{request?.Host}/{source.PicturePath}";
            }
            else
            {
                return "Empty";
            }
        }

    }
}
