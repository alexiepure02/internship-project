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

        public static void AddItemsToEachUser(Dictionary<int, List<int>> items, List<User> users, string propertyName)
        {
            if (propertyName == "friends")
            {
                foreach (User user in users)
                {
                    user.Friends = items[user.Id];
                }
            }
            else if (propertyName == "friendRequests")
            {
                foreach (User user in users)
                {
                    user.FriendRequests = items[user.Id];
                }
            }

        }

        public static void PutUsersIntoJson(List<User> users)
        {
            string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter("../../../Infrastructure/users.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        public static void PutMessagesIntoJson(List<Message> messages)
        {
            string jsonString = JsonSerializer.Serialize(messages, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter("../../../Infrastructure/messages.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        public static void PutFriendsIntoJson(List<User> users)
        {
            Dictionary<int, List<int>> friends = new Dictionary<int, List<int>>();

            foreach (User user in users)
            {
                friends.Add(user.Id, user.Friends);
            }

            string jsonString = JsonSerializer.Serialize(friends, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter("../../../Infrastructure/friends.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        public static void PutFriendRequestsIntoJson(List<User> users)
        {
            Dictionary<int, List<int>> friendRequests = new Dictionary<int, List<int>>();

            foreach (User user in users)
            {
                friendRequests.Add(user.Id, user.FriendRequests);
            }

            string jsonString = JsonSerializer.Serialize(friendRequests, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter("../../../Infrastructure/friendRequests.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        public static void ShowMessagesBetweenTwoUsers(User user1, User user2, List<Message> messages)
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

        public static User? Login(List<User> users)
        {
            Console.Write("username: ");
            string? username = Console.ReadLine();
            Console.Write("password: ");
            string? password = Console.ReadLine();

            User? loggedUser = users.Find((user) => username == user.Username && password == user.Password);

            return loggedUser == null ? throw new UserNotFoundException(username) : loggedUser;
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

        public static void PreFriendsMenu(User loggedUser, List<User> users, List<Message> messages)
        {
            int choice;
            string? choiceString;

            while (true)
            {

                Console.WriteLine("1. Show friends");
                Console.WriteLine("2. Show friend requests");
                Console.WriteLine("3. Add friend");
                Console.WriteLine("4. Delete friend");
                Console.WriteLine("5. Block friend");

                Console.Write("\nPick your choice: ");

                // can be made into a function

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);

                // to here

                try
                {
                    if (CheckIfChoiceValid(choice, 4))
                    {

                        // change to switch-case block

                        if (choice == 1)
                        {
                            FriendsMenu(loggedUser, users, messages);
                        }
                        else if (choice == 2)
                        {
                            FriendRequestsMenu(loggedUser, users);
                        }
                        else if (choice == 3)
                        {
                            AddFriendMenu(loggedUser, users);
                        }
                        else if (choice == 4)
                        {
                            DeleteFriendMenu(loggedUser, users);
                        }
                        else if (choice == 5)
                        {
                            BlockFriendMenu(loggedUser);
                        }
                    }
                }
                catch (NumberBetweenException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static void FriendRequestsMenu(User loggedUser, List<User> users)
        {
            int choice;
            string? choiceString;
            bool remove;

            while (true)
            {
                foreach (int id in loggedUser.FriendRequests)
                {
                    Console.WriteLine($"{GetUserById(id, users).DisplayName} - {id}");
                }

                Console.Write("\nType ID of user to accept or -ID to decline: ");

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                if (choiceString[0] == '-')
                {
                    remove = true;
                    choiceString = choiceString[1..];
                }
                else
                {
                    remove = false;
                }
                choice = ConvertInputToInt(choiceString);

                try
                {
                    ValidateFriendId(choice, loggedUser, users);

                    if (remove == false)
                    {
                        loggedUser.Friends.Add(choice);
                        users.Find(user => user.Id == choice).Friends.Add(loggedUser.Id);
                    }
                    loggedUser.FriendRequests.Remove(choice);

                    break;
                }
                catch (UserNotFoundException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
                catch (UserInFriendsException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
                catch
                {
                    // make this into a custom exception

                    Console.WriteLine($"Error: {choice} is your ID.");
                }
            }

        }

        public static void ValidateFriendId(int id, User loggedUser, List<User> users)
        {
            if (loggedUser.Id == id)
            {
                throw new Exception();
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

        private static void AddFriendMenu(User loggedUser, List<User> users)
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

                    users.Find(user => user.Id == choice).FriendRequests.Add(loggedUser.Id);

                    Console.WriteLine("Request sent successfully.\n");
                    break;

                }
                catch (UserNotFoundException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
                catch (UserInFriendsException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
                catch
                {
                    // make this into a custom exception

                    Console.WriteLine($"Error: {choice} is your ID.");
                }
            }
        }

        private static void DeleteFriendMenu(User loggedUser, List<User> users)
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
                catch (UserNotFoundException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
                catch (UserInFriendsException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
                catch
                {
                    // make this into a custom exception

                    Console.WriteLine($"Error: {choice} is your ID.");
                }
            }
        }
        private static void BlockFriendMenu(User loggedUser)
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
            if (string.IsNullOrWhiteSpace(message))
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

            List<User> users = GetItemsFromJson<List<User>>("users");

            List<Message> messages = GetItemsFromJson<List<Message>>("messages");

            Dictionary<int, List<int>> friends = GetItemsFromJson<Dictionary<int, List<int>>>("friends");
            AddItemsToEachUser(friends, users, "friends");

            Dictionary<int, List<int>> friendRequests = GetItemsFromJson<Dictionary<int, List<int>>>("friendRequests");
            AddItemsToEachUser(friendRequests, users, "friendRequests");


            LoginMenu(users, messages);

            //PutUsersIntoJson(users);
            PutMessagesIntoJson(messages);
            PutFriendsIntoJson(users);
            PutFriendRequestsIntoJson(users);
            return 0;
        }
    }
}