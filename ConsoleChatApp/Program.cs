using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleChatApp
{
    internal class Program
    {
        public static List<User> GetUsersFromJson()
        {
            List<User> users = new List<User>();

            using (StreamReader r = new StreamReader("../../../users.json"))
            {
                string json = r.ReadToEnd();
                users = JsonSerializer.Deserialize<List<User>>(json);
            }

            try
            {
            return users;
            }
            catch (FieldAccessException ex)
            {
                Console.WriteLine("Error with access to the users file\n\n" + ex.Message);
                return new List<User>();
            }
        }

        public static List<Message> GetMessagesFromJson()
        {
            List<Message> messages = new List<Message>();

            using (StreamReader r = new StreamReader("../../../messages.json"))
            {
                string json = r.ReadToEnd();
                messages = JsonSerializer.Deserialize<List<Message>>(json);
            }

            try
            {
                return messages;
            }
            catch (FieldAccessException ex)
            {
                Console.WriteLine("Error with access to the messages file\n\n" + ex.Message);
                return new List<Message>();
            }
        }

        public static void ShowMessagesBetweenTwoUsers(User user1, User user2, List<Message> messages)
        {
            int idUser1 = user1.Id;
            int idUser2 = user2.Id;

            Console.WriteLine(String.Format("{0,-10}{1,60}\n", user1.DisplayName, user2.DisplayName));

            foreach (Message message in messages)
            {
                if (message.IdSender == idUser1 && message.IdReceiver == idUser2)
                {
                    Console.WriteLine(String.Format("{0,70}", message.Text));
                }
                else if (message.IdSender == idUser2 && message.IdReceiver== idUser1)
                {
                    Console.WriteLine(message.Text);
                }
            }
        }

        static int Main(string[] args)
        {
            List<User> users = GetUsersFromJson();
            List<Message> messages = GetMessagesFromJson();

            ShowMessagesBetweenTwoUsers(users[0], users[1], messages);

            return 0;
        }
    }
}