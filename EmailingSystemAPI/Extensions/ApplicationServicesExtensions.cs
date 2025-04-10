using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Repository;
using EmailingSystemAPI.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using EmailingSystemAPI.NotificationService;

namespace EmailingSystemAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection Services)
        {

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            Services.AddHttpContextAccessor();


            return Services;
        }
    }
}
