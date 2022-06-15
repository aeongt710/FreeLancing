using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FreeLancing.Chat
{
    [Authorize]
    public class ChatHub:Hub
    {
        public async Task SendMessage(string message)
        {
            string a = Context.UserIdentifier;
            string b = Clients.ToString();
            //await Clients.Users(new List<string>() { a }).SendAsync("ReceivePublicMessage", message);
            //await Clients.Users().SendAsync(Context.ConnectionId, name);
            //await Clients.All.SendAsync("ReceivePublicMessage", "all");
        }
        public async Task SendSpecificMessage(string toUser, string message)
        {
            string name = Context.User.Identity.Name;
            await Clients.Group(name).SendAsync("ReceivePrivateMessage", name, message);
            await Clients.Group(toUser).SendAsync("ReceivePrivateMessage", name, message);
        }
        public override async Task<Task> OnConnectedAsync()
        {

            string name = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            return base.OnConnectedAsync();
        }
    }
}
