
using EmailingSystem.Repository.Data.Contexts;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Extensions;
using EmailingSystemAPI.Middlewares;
using EmailingSystemAPI.NotificationService;
using HospitalML;
using HospitalML.Extentions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace EmailingSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            // Add Token To Swagger


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Emailing System", Version = "v1" });

                // Adding Bearer Token Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,  // Token will be passed in the Header
                    Description = "Enter Bearer Token",  // Description for Swagger UI
                    Type = SecuritySchemeType.Http,  // The security type for HTTP-based authentication
                    BearerFormat = "JWT",  // Indicating that the token is a JWT
                    Scheme = "Bearer"  // This tells Swagger it's a Bearer token
                });

                // Applying the security requirement for all API endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"  // The ID should match the name of the security definition
                }
            },
            new string[] { }
        }
    });
            });


            builder.Services.AddDbContext<EmailDbContext>( options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                       .UseLazyLoadingProxies();
            });

            builder.Services.ApplicationServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);

            //Overriding Validation Error
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext =>
                {
                    var response = new APIValidationErrorResponse()
                    {
                        Errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToList()
                    };
                    return new BadRequestObjectResult(response);
                });
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()  // Allow any origin
                          .AllowAnyHeader()   // Allow any header
                          .AllowAnyMethod();  // Allow any HTTP method (GET, POST, PUT, DELETE)
                });
            });

            var app = builder.Build();


          

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<EmailDbContext>();

            //HospitalMLSeed.SeedData(context);
            //IdentitySeed.SeedIdentity(services);

            try
            {
                DbAutomaticMigrations.ApplyMigrstions(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error While Applying Migrations.");
            }




            app.UseRouting();
            app.UseCors("AllowAllOrigins");

            if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
            {
                app.UseDeveloperExceptionPage();


                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scoreboard API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.MapHealthChecks("/Health");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<Notification>("/hubs/notify");
            
            app.Run();
        }
    }
}
