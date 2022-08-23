using Domain;

namespace Application
{
    public interface IMessageRepository
    {
        bool ValidateMessage(string message);
        Task CreateMessageAsync(Message message);
        Task<List<Message>> GetMessagesBetweenTwoUsersAsync(int idUser1, int idUser2);
    }
}
