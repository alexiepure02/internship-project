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
            Console.Write("\n>: ");
        }

        public static User? Login(List<User> users)
        {
            Console.Write("username: ");
            string? username = Console.ReadLine();
            Console.Write("password: ");
            string? password = Console.ReadLine();

            User? loggedUser = users.Find((user) => username == user.Username && password == user.Password);

            return loggedUser == null ? throw new UserNotFoundException() : loggedUser;
        }

        public static void LoginMenu(List<User> users, List<Message> messages)
        {
            User? loggedUser;

            while (true)
            {
                try
                {
                    loggedUser = Login(users);
                }
                catch (UserNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }

                users.Remove(loggedUser);       // this warning is bs

                Console.Clear();

                FriendsMenu(loggedUser, users, messages);

                users.Add(loggedUser);
            }
        }

        public static void WriteFriendsMenu(User loggedUser, List<User> users)
        {
            Console.WriteLine("Friends:\n");

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i] != loggedUser)
                {
                    Console.WriteLine($"{i + 1}. {users[i].DisplayName}");
                }
            }
            Console.Write("\nPick someone to send a message to: ");
        }

        public static int ConvertInputToInt(string? receiverIndexString)
        {
            int receiverIndex;
            if (int.TryParse(receiverIndexString, out receiverIndex) == false)
            {
                return -1;
            }
            return receiverIndex;
        }

        public static bool CheckIfChoiceValid(int receiverIndex, int length)
        {
            if (receiverIndex <= 0 || receiverIndex > length)
            {
                throw new NumberBetweenException(length);
            }
            return true;            
        }

        public static void FriendsMenu(User loggedUser, List<User> users, List<Message> messages)
        {
            int receiverIndex;
            string? receiverIndexString;
            
            while (true)
            {
                WriteFriendsMenu(loggedUser, users);

                receiverIndexString = Console.ReadLine();

                Console.Clear();

                if (receiverIndexString == "back") break;

                receiverIndex = ConvertInputToInt(receiverIndexString);
                try
                {
                    if (CheckIfChoiceValid(receiverIndex, users.Count))
                    {
                        MessagesMenu(loggedUser, users[receiverIndex - 1], messages);
                    }
                }
                catch (NumberBetweenException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static bool ContainsProfanity(string message)
        {
            string[] profanity = new string[5] { "idiot", "dumb", "booger", "alligator", "monkey" };

            foreach (string word in profanity)
            {
                if (message.Contains(word)) return true;
            }
            return false;
        }

        public static void CheckIfMessageValid(string? message)
        {
            if (message == null)
            {
                throw new InvalidMessageException("Please type a message to continue.");
            }
            else if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidMessageException("Please type a message to continue.");
            }
            else if (message.Length > 256)
            {
                throw new InvalidMessageException("The message should have a maximum of 256 characters.");
            }
            else if (ContainsProfanity(message))
            {
                throw new InvalidMessageException("The message contains profanity.");
            }
        }

        public static void MessagesMenu(User loggedUser, User Receiver, List<Message> messages)
        {
            string? message;

            while (true)
            {
                ShowMessagesBetweenTwoUsers(loggedUser, Receiver, messages);

                message = Console.ReadLine();

                Console.Clear();

                if (message == "back") break;

                try
                {
                    CheckIfMessageValid(message);

                    messages.Add(new Message() { IdSender = loggedUser.Id, IdReceiver = Receiver.Id, DateTime = DateTime.UtcNow.ToString(), Text = message });
                }
                catch (InvalidMessageException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static int Main(string[] args)
        {
            List<User> users = GetUsersFromJson();
            List<Message> messages = GetMessagesFromJson();

            LoginMenu(users, messages);

            return 0;
        }
    }
}