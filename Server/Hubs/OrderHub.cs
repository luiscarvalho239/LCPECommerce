using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LCPECommerce.Server.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}
