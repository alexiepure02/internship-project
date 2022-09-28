using Domain;
using Microsoft.AspNetCore.SignalR;
using WebPresentation.Dto;

namespace WebPresentation.SignalR
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(MessageGetDto message)
        {
            await Clients.Others.ReceiveMessage(message);

        }
    }
}
