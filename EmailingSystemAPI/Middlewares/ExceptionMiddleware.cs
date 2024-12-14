
using EmailingSystemAPI.Errors;
using System.Text.Json;

namespace EmailingSystemAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment environment;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            this.next = next;
            this.environment = environment;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next.Invoke(httpContext); 

            }
            catch (Exception ex)
            {

                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";
                var response = environment.IsDevelopment() ? new APIExceptionResponse(500, ex.Message, ex.StackTrace?.ToString()) : new APIExceptionResponse(500);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
