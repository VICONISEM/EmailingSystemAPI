using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Repository;
using EmailingSystemAPI.Helper;
using Microsoft.AspNetCore.Mvc;

namespace EmailingSystemAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection Services)
        {

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IConversationRepository), typeof(ConversationRepository));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();


            #region Error Handling
            //Services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = (actionContext) =>
            //    {
            //        var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
            //                                             .SelectMany(P => P.Value.Errors)
            //                                             .Select(E => E.ErrorMessage).ToList();

            //        var response = new APIValidationErrorResponse()
            //        {
            //            Errors = errors
            //        };
            //        return new BadRequestObjectResult(response);

            //    };
            //});
            #endregion

            return Services;
        }
    }
}
