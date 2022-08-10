using ConsoleChatApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IMessageRepository
    {
        void WriteMessagesBetweenTwoUsers(int idSender, int idReceiver);
        bool CheckProfanity(string message);
        bool CheckIfMessageValid(string message);
        void AddMessage(int idSender, int idReceiver, string message);
    }
}
