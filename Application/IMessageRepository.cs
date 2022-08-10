using Domain;

namespace Application
{
    public interface IMessageRepository
    {
        void AddMessages(List<Message> messages);
        List<Message> GetMessagesBetweenTwoUsers(int idSender, int idReceiver);
        bool CheckProfanity(string message);
        void CheckIfMessageValid(string message);
        void AddMessage(int idSender, int idReceiver, string message);
    }
}
