using Application;
using Application.Users.GetUserByUsernameAndPassword;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
// temporary
using Domain.Exceptions;
using System.Text.Json;
using Domain;
using System.Reflection;
using System.Net.NetworkInformation;
using Application.Users.AddUsers;
using Application.Users.AddUser;
using Application.Messages.AddMessages;
using Application.Messages.GetMessagesBetweenTwoUsers;
using Application.Users.RemoveUser;
using Application.Users.GetAllDisplayNames;
using Application.Users.GetUserById;
using Application.Users.ValidateIdFriend;
using Application.Users.AcceptOrRemoveFriendRequest;
using Application.Users.SendFriendRequest;
using Application.Users.RemoveFriend;
using Application.Users.GetUsersCount;
using Application.Messages.CheckIfMessageValid;
using Application.Messages.AddMessage;
using Application.Messages.GetMessages;
using Application.Users.GetUsers;
// to here

namespace Presentation
{
    public class Program
    {
        public static User Login(IMediator mediator)
        {
            Console.Write("username: ");
            string? username = Console.ReadLine();
            Console.Write("password: ");
            string? password = Console.ReadLine();

            try
            {
                User user = mediator.Send(new GetUserByUsernameAndPassword
                {
                    Username = username,
                    Password = password
                }).Result;

                return user;
            }
            catch
            {
                throw;
            }
        }

        public static void LoginMenu(IMediator mediator)
        {
            User loggedUser;

            while (true)
            {
                try
                {
                    loggedUser = Login(mediator);

                    Console.Clear();

                    mediator.Send(new RemoveUser
                    {
                        User = loggedUser
                    });

                    PreFriendsMenu(loggedUser, mediator);

                    mediator.Send(new AddUser
                    {
                        User = loggedUser
                    });
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    break;
                }
            }
        }

        public static int ConvertInputToInt(string choiceString)
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

        public static void PreFriendsMenu(User loggedUser, IMediator mediator)
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
                    if (CheckIfChoiceValid(choice, 5))
                    {
                        switch (choice)
                        {
                            case 1:
                                FriendsMenu(loggedUser, mediator);
                                break;
                            case 2:
                                FriendRequestsMenu(loggedUser, mediator);
                                break;
                            case 3:
                                AddFriendMenu(loggedUser, mediator);
                                break;
                            case 4:
                                DeleteFriendMenu(loggedUser, mediator);
                                break;
                            case 5:
                                BlockFriendMenu(loggedUser, mediator);
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
            if (choice == "") return false;
            return choice[0] == '-' ? true : false;
        }

        public static void WriteFriendRequestsMenu(User loggedUser, IMediator mediator)
        {
            foreach (int id in loggedUser.FriendRequests)
            {
                string displayName = mediator.Send(new GetUserById { Id = id}).Result.DisplayName;
                Console.WriteLine($"{displayName} - {id}");
            }

            Console.Write("\nType ID of user to accept or -ID to decline: ");
        }

        public static void FriendRequestsMenu(User loggedUser, IMediator mediator)
        {
            int choice;
            string? choiceString;
            bool removeFriendReq;

            while (true)
            {
                WriteFriendRequestsMenu(loggedUser, mediator);

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                removeFriendReq = CheckAcceptOrRemove(choiceString);

                if (removeFriendReq) choiceString = choiceString[1..];

                choice = ConvertInputToInt(choiceString);

                try
                {
                    Task<Unit> command = mediator.Send(new ValidateIdFriend
                    {
                        LoggedUser = loggedUser,
                        idFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception;

                    mediator.Send(new AcceptOrRemoveFriendRequest
                    {
                        LoggedUser = loggedUser,
                        idFriend = choice,
                        removeFriendRequest = removeFriendReq
                    });

                    if (removeFriendReq == true)
                    {
                        Console.WriteLine("Request removed succesfully.");
                    }
                    else
                    {
                        Console.WriteLine("Request accepted succesfully.");
                    }
                    Console.WriteLine();
                    break;
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.InnerException.Message + "\n");
                }
            }
        }

        public static void AddFriendMenu(User loggedUser, IMediator mediator)
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
                    Task<Unit> command = mediator.Send(new ValidateIdFriend
                    {
                        LoggedUser = loggedUser,
                        idFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception;

                    mediator.Send(new SendFriendRequest
                    {
                        LoggedUser = loggedUser,
                        idFriend = choice
                    });

                    Console.WriteLine("Request sent successfully.\n");
                    break;

                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.InnerException.Message + "\n");
                }
            }
        }

        public static void ValidateDeleteFriendId(User loggedUser, int choice, IMediator mediator)
        {
            if (loggedUser.Id == choice)
            {
                throw new SameIdException(choice);
            }
            if (mediator.Send(new GetUserById { Id = choice }).Result == null)
            {
                throw new UserNotFoundException(choice);
            }
        }

        public static void WriteDeleteFriendsMenu(User loggedUser, IMediator mediator)
        {
            foreach (int id in loggedUser.Friends)
            {
                string displayName = mediator.Send(new GetUserById { Id = id }).Result.DisplayName;

                Console.WriteLine($"{displayName} - {id}");
            }

            Console.Write("\nIntroduce the ID of the person you want to delete: ");
        }

        public static void DeleteFriendMenu(User loggedUser, IMediator mediator)
        {
            int choice;
            string? choiceString;

            while (true)
            {

                WriteDeleteFriendsMenu(loggedUser, mediator);
                
                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);

                try
                {
                    ValidateDeleteFriendId(loggedUser, choice, mediator);

                    mediator.Send(new RemoveFriend
                    {
                        LoggedUser = loggedUser,
                        IdFriend = choice
                    });
                    
                    Console.WriteLine("Friend removed successfully.\n");
                    break;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }
        
        public static void BlockFriendMenu(User loggedUser, IMediator mediator)
        {
            // to do
        }

        public static void WriteFriendsMenu(User loggedUser, IMediator mediator)
        {
            Console.WriteLine("Friends:\n");

            for (int i = 0; i < loggedUser.Friends.Count; i++)
            {
                string displayName = mediator.Send(new GetUserById { Id = loggedUser.Friends[i] }).Result.DisplayName;

                Console.WriteLine($"{i + 1}. {displayName}");
            }
            Console.Write("\nPick someone to send a message to: ");
        }

        public static void FriendsMenu(User loggedUser, IMediator mediator)
        {
            int choice;
            string? choiceString;

            while (true)
            {
                WriteFriendsMenu(loggedUser, mediator);

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);
                try
                {
                    int numberOfFriends = loggedUser.Friends.Count;

                    if (CheckIfChoiceValid(choice, numberOfFriends))
                    {
                        User friend = mediator.Send(new GetUserById { Id = loggedUser.Friends[choice - 1] }).Result;
                        MessagesMenu(loggedUser, friend, mediator);
                    }
                }
                catch (NumberBetweenException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

    // change this
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

        public static void MessagesMenu(User loggedUser, User friend, IMediator mediator)
        {
            string? sentMessage;

            while (true)
            {
                List<Message> messages = mediator.Send(new GetMessagesBetweenTwoUsers
                {
                    IdSender = loggedUser.Id,
                    IdReceiver = friend.Id
                }).Result;

                WriteMessagesBetweenTwoUsers(loggedUser, friend, messages);

                sentMessage = Console.ReadLine();

                Console.Clear();

                if (sentMessage == "back") break;

                try
                {
                    Task<MediatR.Unit> command = mediator.Send(new CheckIfMessageValid { Message = sentMessage });

                    if (command.IsFaulted) throw command.Exception;
                    
                    mediator.Send(new AddMessage
                    {
                        IdSender = loggedUser.Id,
                        IdReceiver = friend.Id,
                        Message = sentMessage
                    });
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.InnerException.Message + "/n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }
        static int Main(string[] args)
        {
            List<User> users = ManageData.Instance.GetItemsFromJson<List<User>>("users");
            List<Message> messages = ManageData.Instance.GetItemsFromJson<List<Message>>("messages");

            var services = new ServiceCollection()
                .AddScoped<IUserRepository, InMemoryUserRepository>()
                .AddScoped<IMessageRepository, InMemoryMessageRepository>()
                .AddMediatR(typeof(IUserRepository))
                .BuildServiceProvider();

            var mediator = services.GetRequiredService<IMediator>();

            mediator.Send(new AddUsers
            {
                Users = users
            });

            mediator.Send(new AddMessages
            {
                Messages = messages
            });
            
            LoginMenu(mediator);

            users = mediator.Send(new GetUsers()).Result;
            messages = mediator.Send(new GetMessages()).Result;

            ManageData.Instance.PutItemsIntoJson<List<User>>(users, "users");
            ManageData.Instance.PutItemsIntoJson<List<Message>>(messages, "messages");

            return 0;
        }
    }
}