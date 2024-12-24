using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class UserProfileImageResolver : IValueResolver<ApplicationUser, UserDto, string>
    {
        private readonly IHttpContextAccessor configuration;

        public UserProfileImageResolver(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(ApplicationUser source, UserDto destination, string destMember, ResolutionContext context)
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
