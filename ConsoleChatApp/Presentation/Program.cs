using ConsoleChatApp.Domain;
using ConsoleChatApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleChatApp.Presentation
{
    internal class Program
    {
        public static T GetItemsFromJson<T>(string jsonName) where T : new()
        {
            T items = new T();

            using (StreamReader r = new StreamReader($"../../../Infrastructure/{jsonName}.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<T>(json);
            }

            return items;
        }

        public static void PutItemsIntoJson<T>(T items, string jsonName)
        {
            string jsonString = JsonSerializer.Serialize(items, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter($"../../../Infrastructure/{jsonName}.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        public static User FindUser(string username, string password, List<User> users)
        {
            User loggedUser = users.Find((user) => username == user.Username && password == user.Password);

            return loggedUser == null ? throw new UserNotFoundException(username) : loggedUser;
        }

        public static User? Login(List<User> users)
        {
            Console.Write("username: ");
            string? username = Console.ReadLine();
            Console.Write("password: ");
            string? password = Console.ReadLine();

            return FindUser(username, password, users);
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

                Console.Clear();

                users.Remove(loggedUser);

                PreFriendsMenu(loggedUser, users, messages);

                users.Add(loggedUser);
            }
        }

        public static User GetUserById(int id, List<User> users)
        {
            return users.Find(user => user.Id == id);
        }

        public static int ConvertInputToInt(string? choiceString)
        {
            int receiverIndex;
            if (int.TryParse(choiceString, out receiverIndex) == false)
            {
                return -1;
            }
            return receiverIndex;
        }

        public static bool CheckIfChoiceValid(int choice, int length)
        {
            if (choice <= 0 || choice > length)
            {
                throw new NumberBetweenException(length);
            }
            return true;
        }

        public static void WritePreFriendsMenu()
        {
            Console.WriteLine("1. Show friends");
            Console.WriteLine("2. Show friend requests");
            Console.WriteLine("3. Add friend");
            Console.WriteLine("4. Delete friend");
            Console.WriteLine("5. Block friend");

            Console.Write("\nPick your choice: ");
        }

        public static void PreFriendsMenu(User loggedUser, List<User> users, List<Message> messages)
        {
            int choice;
            string? choiceString;

            while (true)
            {
                WritePreFriendsMenu();

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);

                try
                {
                    if (CheckIfChoiceValid(choice, 4))
                    {
                        switch (choice)
                        {
                            case 1:
                                FriendsMenu(loggedUser, users, messages);
                                break;
                            case 2:
                                FriendRequestsMenu(loggedUser, users);
                                break;
                            case 3:
                                AddFriendMenu(loggedUser, users);
                                break;
                            case 4:
                                DeleteFriendMenu(loggedUser, users);
                                break;
                            case 5:
                                BlockFriendMenu(loggedUser);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (NumberBetweenException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static bool CheckAcceptOrRemove(string choice)
        {
            return choice[0] == '-' ? true : false;
        }

        public static void AcceptFriendRequest(User loggedUser, int introducedId, List<User> users)
        {
            loggedUser.Friends.Add(introducedId);
            users.Find(user => user.Id == introducedId).Friends.Add(loggedUser.Id);
        }

        public static void ValidateFriendId(int id, User loggedUser, List<User> users)
        {
            if (loggedUser.Id == id)
            {
                throw new SameIdException(id);
            }
            if (GetUserById(id, users) == null)
            {
                throw new UserNotFoundException(id);
            }
            if (loggedUser.Friends.Contains(id))
            {
                throw new UserInFriendsException(id);
            }
        }

        public static void WriteFriendRequestsMenu(User loggedUser, List<User> users)
        {
            foreach (int id in loggedUser.FriendRequests)
            {
                Console.WriteLine($"{GetUserById(id, users).DisplayName} - {id}");
            }

            Console.Write("\nType ID of user to accept or -ID to decline: ");
        }

        public static void FriendRequestsMenu(User loggedUser, List<User> users)
        {
            int choice;
            string? choiceString;
            bool remove;

            while (true)
            {
                WriteFriendRequestsMenu(loggedUser, users);

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                remove = CheckAcceptOrRemove(choiceString);

                if (remove) choiceString = choiceString[1..];

                choice = ConvertInputToInt(choiceString);

                try
                {
                    ValidateFriendId(choice, loggedUser, users);

                    if (remove == false)
                        AcceptFriendRequest(loggedUser, choice, users);

                    loggedUser.FriendRequests.Remove(choice);

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static void SendFriendRequest(User loggedUser, int introducedId, List<User> users)
        {
            users.Find(user => user.Id == introducedId).FriendRequests.Add(loggedUser.Id);
        }

        public static void AddFriendMenu(User loggedUser, List<User> users)
        {
            int choice;
            string? choiceString;

            while (true)
            {
                Console.Write("Introduce the ID of the person you want to add: ");

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);

                try
                {
                    ValidateFriendId(choice, loggedUser, users);
                    SendFriendRequest(loggedUser, choice, users);

                    Console.WriteLine("Request sent successfully.\n");
                    break;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static void DeleteFriendMenu(User loggedUser, List<User> users)
        {
            int choice;
            string? choiceString;

            while (true)
            {

                foreach (int id in loggedUser.Friends)
                {
                    Console.WriteLine($"{GetUserById(id, users).DisplayName} - {id}");
                }

                Console.Write("\nIntroduce the ID of the person you want to delete: ");

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);

                try
                {
                    // create validation function for this

                    if (loggedUser.Id == choice)
                    {
                        throw new Exception();
                    }
                    if (GetUserById(choice, users) == null)
                    {
                        throw new UserNotFoundException(choice);
                    }

                    // to here

                    loggedUser.Friends.Remove(choice);
                    users.Find(user => user.Id == choice).Friends.Remove(loggedUser.Id);
                    Console.WriteLine("Friend removed successfully.\n");
                    break;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }
        
        public static void BlockFriendMenu(User loggedUser)
        {
            // to do
        }

        public static void WriteFriendsMenu(User loggedUser, List<User> users)
        {
            Console.WriteLine("Friends:\n");

            for (int i = 0; i < loggedUser.Friends.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {GetUserById(loggedUser.Friends[i], users).DisplayName}");
            }
            Console.Write("\nPick someone to send a message to: ");
        }

        public static void FriendsMenu(User loggedUser, List<User> users, List<Message> messages)
        {
            int choice;
            string? choiceString;

            while (true)
            {
                WriteFriendsMenu(loggedUser, users);

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);
                try
                {
                    if (CheckIfChoiceValid(choice, users.Count))
                    {
                        MessagesMenu(loggedUser, GetUserById(loggedUser.Friends[choice - 1], users), messages);
                    }
                }
                catch (NumberBetweenException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static void WriteMessagesBetweenTwoUsers(User user1, User user2, List<Message> messages)
        {
            int idUser1 = user1.Id;
            int idUser2 = user2.Id;

            Console.WriteLine($"{user2.DisplayName}:");

            foreach (Message message in messages)
            {
                if (message.IdSender == idUser1 && message.IdReceiver == idUser2)
                {
                    Console.WriteLine(string.Format("{0,70}", message.Text));
                }
                else if (message.IdSender == idUser2 && message.IdReceiver == idUser1)
                {
                    Console.WriteLine(message.Text);
                }
            }
            Console.Write("\n>: ");
        }

        public static bool CheckProfanity(string message)
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

        public static void AddMessage(List<Message> messages, User loggedUser, User Receiver, string message)
        {
            messages.Add(new Message() { IdSender = loggedUser.Id, IdReceiver = Receiver.Id, DateTime = DateTime.UtcNow.ToString(), Text = message });
        }

        public static void MessagesMenu(User loggedUser, User Receiver, List<Message> messages)
        {
            string? message;

            while (true)
            {
                WriteMessagesBetweenTwoUsers(loggedUser, Receiver, messages);

                message = Console.ReadLine();

                Console.Clear();

                if (message == "back") break;

                try
                {
                    CheckIfMessageValid(message);
                    AddMessage(messages, loggedUser, Receiver, message);
                }
                catch (InvalidMessageException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        static int Main(string[] args)
        {

            List<User> users = GetItemsFromJson<List<User>>("users");
            List<Message> messages = GetItemsFromJson<List<Message>>("messages");

            LoginMenu(users, messages);

            PutItemsIntoJson<List<User>>(users, "users");
            PutItemsIntoJson<List<Message>>(messages, "messages");

            return 0;
        }
    }
}