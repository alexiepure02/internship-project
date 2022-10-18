using Domain;

namespace Application
{
    public interface IMessageRepository
    {
        bool ValidateMessage(string message);
        Task CreateMessageAsync(Message message);
        Task<List<Message>> GetMessagesBetweenTwoUsersAsync(int idUser1, int idUser2);
        Task<Message> GetMessageByIdAsync(int id);
        Task<int> GetNumberOfMessagesBetweenTwoUsersAsync(int idUser1, int idUser2);
        Task<List<Message>> GetSomeMessagesFromOffset(int idUser1, int idUser2, int offset, int numberOfMessages);
    }
}
