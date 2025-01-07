using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.User;

namespace EmailingSystemAPI.Helper
{
    public class ValidUserProfileResolver : IValueResolver<ApplicationUser, AllowedUserDto, string>
    {
        private readonly IHttpContextAccessor configuration;

        public ValidUserProfileResolver(IHttpContextAccessor configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(ApplicationUser source, AllowedUserDto destination, string destMember, ResolutionContext context)
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
