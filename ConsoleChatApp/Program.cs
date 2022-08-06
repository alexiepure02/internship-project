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
        public static List<User>? GetUsersFromJson()
        {
            List<User> users = new List<User>();

            using (StreamReader r = new StreamReader("../../../users.json"))
            {
                string json = r.ReadToEnd();
                users = JsonSerializer.Deserialize<List<User>>(json);
            }

            return users;
        }

        public static List<Message> GetMessagesFromJson()
        {
            List<Message> messages = new List<Message>();

            using (StreamReader r = new StreamReader("../../../messages.json"))
            {
                string json = r.ReadToEnd();
                messages = JsonSerializer.Deserialize<List<Message>>(json);
            }

            return messages;
        }

        public static Dictionary<int, List<int>> GetFriendsFromJson()
        {
            Dictionary<int, List<int>> friends = new Dictionary<int, List<int>>();

            using (StreamReader r = new StreamReader("../../../friends.json"))
            {
                string json = r.ReadToEnd();
                friends = JsonSerializer.Deserialize<Dictionary<int, List<int>>>(json);
            }

            return friends;
        }

        public static Dictionary<int, List<int>> GetFriendRequestsFromJson()
        {
            Dictionary<int, List<int>> friendRequests = new Dictionary<int, List<int>>();

            using (StreamReader r = new StreamReader("../../../friendRequests.json"))
            {
                string json = r.ReadToEnd();
                friendRequests = JsonSerializer.Deserialize<Dictionary<int, List<int>>>(json);
            }

            return friendRequests;
        }

        public static void AddFriendsToEachUser(Dictionary<int, List<int>> friends, List<User> users)
        {
            foreach (User user in users)
            {
                user.Friends = friends[user.Id];
            }
        }

        public static void AddFriendRequestsToEachUser(Dictionary<int, List<int>> friendRequests, List<User> users)
        {
            foreach (User user in users)
            {
                user.FriendRequests = friendRequests[user.Id];
            }
        }

        public static void PutUsersIntoJson(List<User> users)
        {
            string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter("../../../users.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        public static void PutMessagesIntoJson(List<Message> messages)
        {
            string jsonString = JsonSerializer.Serialize(messages, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter("../../../messages.json"))
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

            using (StreamWriter outputFile = new StreamWriter("../../../friends.json"))
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

            using (StreamWriter outputFile = new StreamWriter("../../../friendRequests.json"))
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
                            DeleteFriendMenu(loggedUser);
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

                    Console.WriteLine("Request sent successfully.");
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

        private static void DeleteFriendMenu(User loggedUser)
        {
            // to do
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
            List<User> users = GetUsersFromJson();
            
            Dictionary<int, List<int>> friends = GetFriendsFromJson();
            AddFriendsToEachUser(friends, users);

            Dictionary<int, List<int>> friendRequests = GetFriendRequestsFromJson();
            AddFriendRequestsToEachUser(friendRequests, users);

            List<Message> messages = GetMessagesFromJson();
 
            LoginMenu(users, messages);

            //PutUsersIntoJson(users);
            PutMessagesIntoJson(messages);
            PutFriendsIntoJson(users);
            PutFriendRequestsIntoJson(users);
            return 0;
        }
    }
}