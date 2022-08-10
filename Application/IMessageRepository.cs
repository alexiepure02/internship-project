using Domain;

namespace Application
{
    public interface IMessageRepository
    {
        List<Message> GetMessagesBetweenTwoUsers(int idSender, int idReceiver);
        bool CheckProfanity(string message);
        void CheckIfMessageValid(string message);
        void AddMessage(int idSender, int idReceiver, string message);
    }
}
