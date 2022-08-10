using Application;
using ConsoleChatApp.Domain.Exceptions;
using Domain;

namespace Infrastructure
{
    public class InMemoryMessageRepository : IMessageRepository
    {
        private readonly List<Message> _messages = new();

        public InMemoryMessageRepository(List<Message> messages)
        {
            _messages = messages;
        }

        public List<Message> GetMessagesBetweenTwoUsers(int idSender, int idReceiver)
        {
            List<Message> newMessages;

            newMessages = _messages.FindAll(message => message.IdSender == idSender && message.IdReceiver == idReceiver);

            return newMessages;
        }
        public bool CheckProfanity(string message)
        {
            string[] profanity = new string[5] { "idiot", "dumb", "booger", "alligator", "monkey" };

            foreach (string word in profanity)
            {
                if (message.Contains(word)) return true;
            }
            return false;
        }
        public void CheckIfMessageValid(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidMessageException("Please type a message to continue.");
            }
            else if (message.Length > 256)
            {
                throw new InvalidMessageException("The message should have a maximum of 256 characters.");
            }
            else if (CheckProfanity(message))
            {
                throw new InvalidMessageException("The message contains profanity.");
            }
        }
        public void AddMessage(int idSender, int idReceiver, string message)
        {
            _messages.Add(new Message() { IdSender = idSender, IdReceiver = idReceiver, DateTime = DateTime.UtcNow.ToString(), Text = message });

        }
    }
}
