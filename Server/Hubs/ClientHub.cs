using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LCPECommerce.Server.Hubs
{
    public class ClientHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}
