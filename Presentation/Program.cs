using Application;
using Application.Users.GetUserByUsernameAndPassword;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
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
using Application.Users.UpdateFriendRequest;
using Application.Users.SendFriendRequest;
using Application.Users.RemoveFriend;
using Application.Users.GetUsersCount;
using Application.Messages.CheckIfMessageValid;
using Application.Messages.AddMessage;
using Application.Messages.GetMessages;
using Application.Users.GetUsers;
using Application.Users.CheckIfFriendRequestExists;
using Application.Users.GetFriendOfUser;
using Application.Users.GetFriendRequestOfUser;
using Application.Users.ValidateNewUser;

namespace Presentation
{
    public class Program
    {
        public static void WriteFirstMenu()
        {
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("0. Exit");
            Console.Write("\nPick your choice: ");
        }
        public static void FirstMenu(IMediator mediator, AppDbContext context)
        {
            string choice;

            while (true)
            {
                WriteFirstMenu();

                choice = Console.ReadLine();

                Console.Clear();

                if (choice == "0") break;

                if (choice == "1")
                    LoginMenu(mediator, context);
                else if (choice == "2")
                    RegisterMenu(mediator, context);
                else
                    Console.WriteLine("Pick between 1 and 2.\n");
            }
        }

        public static User Login(IMediator mediator)
        {
            Console.Write("username: ");
            string? username = Console.ReadLine();
            Console.Write("password: ");
            string? password = Console.ReadLine();

                User user = mediator.Send(new GetUserByUsernameAndPassword
                {
                    Username = username,
                    Password = password
                }).Result;

            // i have to check for null here because, if i check it in LoginMenu,
            // i lose the typed username, so i have to throw the exception here

            return user == null ? throw new UserNotFoundException(username) : user;
        }

        public static void LoginMenu(IMediator mediator, AppDbContext context)
        {
            User loggedUser;

            Console.Clear();

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

                    PreFriendsMenu(loggedUser, mediator, context);

                    mediator.Send(new AddUser
                    {
                        User = loggedUser
                    });
                }
                catch (UserNotFoundException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message + "\n");
                    break;
                }
            }
        }

        public static void RegisterMenu(IMediator mediator, AppDbContext context)
        {
            while (true)
            {
                Console.Write("username: ");
                string? username = Console.ReadLine();
                Console.Write("password: ");
                string? password = Console.ReadLine();
                Console.Write("display name: ");
                string? displayName = Console.ReadLine();

                Console.Clear();

                int numberOfUsers = mediator.Send(new GetAllDisplayNames()).Result.Count();

                User user = new User
                {
                    ID = numberOfUsers + 1,
                    Username = username,
                    Password = password,
                    DisplayName = displayName,
                    Messages = new List<Message>(),
                    Friends = new List<Friends>(),
                    FriendRequests = new List<FriendRequests>()
                };

                string result = mediator.Send(new ValidateNewUser
                {
                    User = user
                }).Result;

                if (result == "all good")
                {
                    mediator.Send(new AddUser { User = user });

                    // database query

                    context.Add(user);
                    context.SaveChanges();

                    break;
                }

                Console.WriteLine(result + "\n");

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

        public static void PreFriendsMenu(User loggedUser, IMediator mediator, AppDbContext context)
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
                                FriendsMenu(loggedUser, mediator, context);
                                break;
                            case 2:
                                FriendRequestsMenu(loggedUser, mediator, context);
                                break;
                            case 3:
                                AddFriendMenu(loggedUser, mediator,context);
                                break;
                            case 4:
                                DeleteFriendMenu(loggedUser, mediator, context);
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
            return choice[0] == '-' ? false: true;
        }

        public static void WriteFriendRequestsMenu(User loggedUser, IMediator mediator)
        {
            foreach (var user in loggedUser.FriendRequests)
            {
                int id = user.IDRequester;
                string displayName = mediator.Send(new GetUserById { Id = id}).Result.DisplayName;
                Console.WriteLine($"{displayName} - {id}");
            }

            Console.Write("\nType ID of user to accept or -ID to decline: ");
        }

        public static void FriendRequestsMenu(User loggedUser, IMediator mediator, AppDbContext context)
        {
            int choice;
            string? choiceString;
            bool accepted;

            while (true)
            {
                WriteFriendRequestsMenu(loggedUser, mediator);

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                try
                {
                    if (choiceString == "")
                    {
                        throw new UserNotFoundException(-1);
                    }
                    accepted = CheckAcceptOrRemove(choiceString);

                    // exception unhandled
                    // System.ArgumentOutOfRangeException
                    // Message = startIndex cannot be larger than length of string. (Parameter 'startIndex')
                    if (accepted == false) choiceString = choiceString[1..];

                    choice = ConvertInputToInt(choiceString);

                    Task<Unit> command = mediator.Send(new ValidateIdFriend
                    {
                        LoggedUser = loggedUser,
                        IdFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception;

                    User friend = mediator.Send(new GetUserById
                    {
                        Id = choice
                    }).Result;

                    bool friendRequestExists = mediator.Send(new CheckIfFriendRequestExists
                    {
                        LoggedUser = loggedUser,
                        Friend = friend
                    }).Result;

                    if (!friendRequestExists)
                    {
                        throw new UserNotFoundException(-1);
                    }

                    // save friend reference in order to be able to
                    // delete from database

                    FriendRequests friendRequest = mediator.Send(new GetFriendRequestOfUser
                    {
                        User = loggedUser,
                        Friend = friend
                    }).Result;

                    mediator.Send(new UpdateFriendRequest
                    {
                        LoggedUser = loggedUser,
                        Friend = friend,
                        Accepted = accepted
                    });

                    // database query

                    context.Add(new Friends
                    {
                        IDUser = loggedUser.ID,
                        IDFriend = friend.ID
                    });
                    context.Add(new Friends
                    {
                        IDUser = friend.ID,
                        IDFriend = loggedUser.ID
                    });

                    // exception thrown
                    // id has temporary value
                    // potential fix: get friend request from database, not from list

                    context.Remove(friendRequest);
                    context.SaveChanges();

                    if (accepted == false)
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
                catch (UserNotFoundException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static void AddFriendMenu(User loggedUser, IMediator mediator, AppDbContext context)
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
                        IdFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception;

                    // before sending the friend request,
                    // check if it doesn't already exist

                    User friend = mediator.Send(new GetUserById { Id = choice }).Result;

                    bool exists = mediator.Send(new CheckIfFriendRequestExists
                    {
                        LoggedUser = friend,
                        Friend = loggedUser
                    }).Result;

                    if (!exists)
                    {
                        mediator.Send(new SendFriendRequest
                        {
                            LoggedUser = loggedUser,
                            idFriend = choice
                        });

                        // database query

                        context.Add(new FriendRequests
                        {
                            IDUser = choice,
                            IDRequester = loggedUser.ID
                        });
                        context.SaveChanges();
                    }

                    Console.WriteLine("Request sent successfully.\n");
                    break;

                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.InnerException.Message + "\n");
                }
            }
        }

        public static void WriteDeleteFriendsMenu(User loggedUser, IMediator mediator)
        {
            foreach (var user in loggedUser.Friends)
            {
                int id = user.IDFriend;
                string displayName = mediator.Send(new GetUserById { Id = id }).Result.DisplayName;

                Console.WriteLine($"{displayName} - {id}");
            }

            Console.Write("\nIntroduce the ID of the person you want to delete: ");
        }

        public static void DeleteFriendMenu(User loggedUser, IMediator mediator, AppDbContext context)
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
                    // if AggregateException.InnerException == UserInFriendsException
                    //      remove friend
                    // else
                    //      cw exception message

                    Task<Unit> command = mediator.Send(new ValidateIdFriend
                    {
                        LoggedUser = loggedUser,
                        IdFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception.InnerException;

                    // introduced id doesn't belong to any user in friends list

                    throw new UserNotFoundException(choice);
                }
                catch (UserInFriendsException)
                {
                    User friend = mediator.Send(new GetUserById
                    {
                        Id = choice
                    }).Result;

                    // save friend reference in order to be able to
                    // delete from database

                    Friends friend1 = mediator.Send(new GetFriendOfUser
                    {
                        User = loggedUser,
                        Friend = friend
                    }).Result;
                    Friends friend2 = mediator.Send(new GetFriendOfUser
                    {
                        User = friend,
                        Friend = loggedUser
                    }).Result;

                    mediator.Send(new RemoveFriend
                    {
                        LoggedUser = loggedUser,
                        Friend = friend
                    });

                    // database query

                    context.Remove(friend1);
                    context.Remove(friend2);
                    context.SaveChanges();

                    Console.WriteLine("Friend removed successfully.\n");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }
        
        public static void WriteFriendsMenu(User loggedUser, IMediator mediator)
        {
            Console.WriteLine("Friends:\n");

            for (int i = 0; i < loggedUser.Friends.Count; i++)
            {
                string displayName = mediator.Send(new GetUserById { Id = loggedUser.Friends[i].IDFriend }).Result.DisplayName;

                Console.WriteLine($"{i + 1}. {displayName}");
            }
            Console.Write("\nPick someone to send a message to: ");
        }

        public static void FriendsMenu(User loggedUser, IMediator mediator, AppDbContext context)
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
                        User friend = mediator.Send(new GetUserById { Id = loggedUser.Friends[choice - 1].IDFriend }).Result;
                        MessagesMenu(loggedUser, friend, mediator, context);
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
            int idUser1 = user1.ID;
            int idUser2 = user2.ID;

            Console.WriteLine($"{user2.DisplayName}:");

            foreach (Message message in messages)
            {
                if (message.IDSender == idUser1 && message.IDReceiver == idUser2)
                {
                    Console.WriteLine(string.Format("{0,70}", message.Text));
                }
                else if (message.IDSender == idUser2 && message.IDReceiver == idUser1)
                {
                    Console.WriteLine(message.Text);
                }
            }
            Console.Write("\n>: ");
        }

        public static void MessagesMenu(User loggedUser, User friend, IMediator mediator, AppDbContext context)
        {
            string? sentMessage;

            while (true)
            {
                List<Message> messages = mediator.Send(new GetMessagesBetweenTwoUsers
                {
                    IdSender = loggedUser.ID,
                    IdReceiver = friend.ID
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
                        Sender = loggedUser,
                        Receiver = friend,
                        Message = sentMessage
                    });

                    // query

                    context.Add(new Message
                    {
                        IDSender = loggedUser.ID,
                        IDReceiver = friend.ID,
                        Text = sentMessage,
                        DateTime = DateTime.UtcNow.ToString()
                    });
                    context.SaveChanges();

                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.InnerException.Message + "\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }
        
        static int Main(string[] args)
        {
            //List<User> users = ManageData.Instance.GetItemsFromJson<List<User>>("users");
            //List<Message> messages = ManageData.Instance.GetItemsFromJson<List<Message>>("messages");

            // adapted domain classes to make relationships in ef core
            // and now the json data is no longer compatible with the classes

            List<User> usersData = new List<User>
            {
                new User
                {
                    ID = 1,
                    Username = "alexiepure",
                    Password = "1234",
                    DisplayName = "Alex",
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            IDSender = 1,
                            IDReceiver = 2,
                            Text = "Hey! What's up?",
                            DateTime = "8/21/2022 1:34:08 PM"
                        },
                        new Message
                        {
                            IDSender = 1,
                            IDReceiver = 2,
                            Text = "Nothing much. How's work?",
                            DateTime = "8/21/2022 1:34:10 PM"
                        },
                        new Message
                        {
                            IDSender = 1,
                            IDReceiver = 2,
                            Text = "Oh that sounds cool.",
                            DateTime = "8/21/2022 1:34:12 PM"
                        },
                        new Message
                        {
                            IDSender = 1,
                            IDReceiver = 2,
                            Text = "Ok I gotta go. Talk to you later!",
                            DateTime = "8/21/2022 1:34:13 PM"
                        },
                        new Message
                        {
                            IDSender = 1,
                            IDReceiver = 2,
                            Text = "I won't. I'll be there. Bye!",
                            DateTime = "8/21/2022 1:34:15 PM"
                        }
                    },
                    MainUserFriends = new List<Friends>(),
                    Friends = new List<Friends>
                    {
                        new Friends
                        {
                            IDUser = 1,
                            IDFriend = 2
                        }
                    },
                    FriendRequests = new List<FriendRequests>()
                },
                new User
                {
                    ID = 2,
                    Username = "andrei1",
                    Password = "1234",
                    DisplayName = "Andrei",
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            IDSender = 2,
                            IDReceiver = 1,
                            Text = "Oh hey. Just chilling. What's up with you?",
                            DateTime = "8/21/2022 1:34:09 PM"
                        },
                        new Message
                        {
                            IDSender = 2,
                            IDReceiver = 1,
                            Text = "Work is good. I'm currently working on a project.",
                            DateTime = "8/21/2022 1:34:11 PM"
                        },
                        new Message
                        {
                            IDSender = 2,
                            IDReceiver = 1,
                            Text = "Ok. Don't forget about our meeting at 3. See you there!",
                            DateTime = "8/21/2022 1:34:14 PM"
                        },
                    },
                    MainUserFriends = new List<Friends>(),
                    Friends = new List<Friends>
                    {
                        new Friends
                        {
                            IDUser = 2,
                            IDFriend = 1
                        }
                    },
                    FriendRequests = new List<FriendRequests>()
                },
                new User
                {
                    ID = 3,
                    Username = "maria",
                    Password = "1234",
                    DisplayName = "Maria",
                    Messages = new List<Message>
                    {
                    },
                    MainUserFriends = new List<Friends>
                    {
                    },
                    Friends = new List<Friends>
                    {
                    },
                    FriendRequests = new List<FriendRequests>
                    {
                        new FriendRequests
                        {
                            IDUser = 3,
                            IDRequester = 1
                        }
                    }                }
            };

            var context = new AppDbContext();

            // add data to database
            
            /*context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Add(usersData[0]);
            context.Add(usersData[1]);
            context.Add(usersData[2]);

            context.SaveChanges();*/

            var users = context.Users
                .Select(x => new User
                {
                    ID = x.ID,
                    Username = x.Username,
                    Password = x.Password,
                    DisplayName = x.DisplayName,
                    MainUserFriends = x.MainUserFriends,
                    Friends = x.Friends,
                    FriendRequests = x.FriendRequests,
                    Messages = x.Messages
                })
                .ToList();

            List<Message> messages = new();

            foreach (User user in usersData)
            {
                foreach (Message message in user.Messages)
                {
                    messages.Add(message);
                }
            }

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

            messages = messages.OrderBy(x => x.DateTime).ToList();

            mediator.Send(new AddMessages
            {
                Messages = messages
            });
            
            FirstMenu(mediator, context);

            // json files
            // no longer working bcs i changed domain classes

            //ManageData.Instance.PutItemsIntoJson<List<User>>(users, "users");
            //ManageData.Instance.PutItemsIntoJson<List<Message>>(messages, "messages");

            return 0;
        }
    }
}