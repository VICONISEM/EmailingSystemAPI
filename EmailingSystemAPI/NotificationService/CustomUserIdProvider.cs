using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EmailingSystemAPI.NotificationService
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            Console.WriteLine(connection.User?.FindFirst(ClaimTypes.Email)?.Value);
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
