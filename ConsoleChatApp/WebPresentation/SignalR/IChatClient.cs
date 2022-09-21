using Domain;
using WebPresentation.Dto;

namespace WebPresentation.SignalR
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessagePutPostDto message);
    }
}
