using Domain;
using Microsoft.AspNetCore.SignalR;
using WebPresentation.Dto;

namespace WebPresentation.SignalR
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(MessagePutPostDto message)
        {
            await Clients.All.ReceiveMessage(message);

        }
    }
}
