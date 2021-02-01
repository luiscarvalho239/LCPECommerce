using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LCPECommerce.Server.Hubs
{
    public class OptionHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}
