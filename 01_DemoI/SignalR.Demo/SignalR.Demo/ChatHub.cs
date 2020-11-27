using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalR.Demo
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string chatMessage)
        {
            await Clients.All.SendAsync("NewMessage", chatMessage);
        }
    }
}