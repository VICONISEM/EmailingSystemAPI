using EmailingSystemAPI.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace EmailingSystemAPI.NotificationService
{
    public class Notification : Hub
    {
        //public async Task SendNotification(string Email,N str)
        //{
        //    await Clients.User(Email).SendAsync("Notification", str );
        //}
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"User Email conected {Context.UserIdentifier}");  
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"User Email desconnected {Context.UserIdentifier}");
            return base.OnDisconnectedAsync(exception);
        }

    }
}
