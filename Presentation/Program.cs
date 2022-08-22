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
using Application.Queries.GetUserByAccountQuery;
using Application.Queries.GetAllUsersQuery;
using Application.Commands.AddUserCommand;
using Application.Queries.GetUserByIdQuery;
using Application.Queries.GetFriendRequestOfUserQuery;
using Application.Queries.CheckIfFriendRequestExistsQuery;
using Application.Commands.UpdateFriendRequestCommand;
using Application.Commands.AddFriendRequestCommand;
using Application.Commands.RemoveFriendCommand;
using Application.Commands.AddMessageCommand;
using Application.Queries.GetAllFriendsOfUserQuery;
using Application.Queries.GetAllFriendRequestsOfUserQuery;
using Application.Queries.GetMessagesBetweenTwoUsersQuery;
using Application.Queries.CheckIfFriendExistsQuery;

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
        public static void FirstMenu(IMediator mediator)
        {
            string choice;

            while (true)
            {
                WriteFirstMenu();

                choice = Console.ReadLine();

                Console.Clear();

                if (choice == "0") break;

                if (choice == "1")
                    LoginMenu(mediator);
                else if (choice == "2")
                    RegisterMenu(mediator);
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

            User user = mediator.Send(new GetUserByAccountQuery
            {
                Username = username,
                Password = password
            }).Result;

            // i have to check for null here because, if i check it in LoginMenu,
            // i lose the typed username, so i have to throw the exception here

            return user == null ? throw new UserNotFoundException(username) : user;
        }

        public static void LoginMenu(IMediator mediator)
        {
            User loggedUser;

            Console.Clear();

            while (true)
            {
                try
                {
                    loggedUser = Login(mediator);
                    
                    Console.Clear();

                    PreFriendsMenu(loggedUser.ID, mediator);
                }
                catch (UserNotFoundException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message + "\n");
                    break;
                }
            }
        }

        public static void RegisterMenu(IMediator mediator)
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

                int numberOfUsers = mediator.Send(new GetAllUsersQuery()).Result.Count();

                // int numberOfUsers = mediator.Send(new GetAllDisplayNames()).Result.Count();

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
                    //mediator.Send(new AddUser { User = user });

                    mediator.Send(new AddUserCommand
                    {
                        User = user
                    });

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

        public static void PreFriendsMenu(int idUser, IMediator mediator)
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
                                FriendsMenu(idUser, mediator);
                                break;
                            case 2:
                                FriendRequestsMenu(idUser, mediator);
                                break;
                            case 3:
                                AddFriendMenu(idUser, mediator);
                                break;
                            case 4:
                                DeleteFriendMenu(idUser, mediator);
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

        public static void WriteFriendRequestsMenu(int idUser, IMediator mediator)
        {

            // get all friends of user query

            List<FriendRequests> friendRequests = mediator.Send(new GetAllFriendRequestsOfUserQuery
            {
                IDUser = idUser
            }).Result;

            foreach (var friendRequest in friendRequests)
            {
                string displayName = mediator.Send(new GetUserByIdQuery
                {
                    Id = friendRequest.IDRequester
                }).Result.DisplayName;
                
                //string displayName = mediator.Send(new GetUserById { Id = id}).Result.DisplayName;
                
                Console.WriteLine($"{displayName} - {friendRequest.IDRequester}");
            }

            Console.Write("\nType ID of user to accept or -ID to decline: ");
        }

        public static async void FriendRequestsMenu(int idUser, IMediator mediator)
        {
            int choice;
            string? choiceString;
            bool accepted;

            while (true)
            {
                WriteFriendRequestsMenu(idUser, mediator);

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

                    if (accepted == false) choiceString = choiceString[1..];

                    choice = ConvertInputToInt(choiceString);

                    Task<Unit> command = mediator.Send(new ValidateIdFriend
                    {
                        IdUser = idUser,
                        IdFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception;

                    bool friendRequestExists = mediator.Send(new CheckIfFriendRequestExistsQuery
                    {
                        IDUser = idUser,
                        IDRequester = choice
                    }).Result;

                    if (!friendRequestExists)
                    {
                        throw new UserNotFoundException(-1);
                    }

                    await mediator.Send(new UpdateFriendRequestCommand
                    {
                        IDUser = idUser,
                        IDRequester = choice
                    });

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

        public static async void AddFriendMenu(int idUser, IMediator mediator)
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
                        IdUser = idUser,
                        IdFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception;

                    // somehow, someway, i need to use await here but i'm not sure how

                    bool friendExists = mediator.Send(new CheckIfFriendExistsQuery
                    {
                        IDUser = idUser,
                        IDFriend = choice
                    }).Result;

                    if (friendExists)
                    {
                        throw new UserInFriendsException(choice);
                    }

                    bool friendRequestExists = mediator.Send(new CheckIfFriendRequestExistsQuery
                    {
                        IDUser = idUser,
                        IDRequester = choice
                    }).Result;

                    if (!friendRequestExists)
                    {
                        await mediator.Send(new AddFriendRequestCommand
                        {
                            IDUser = idUser,
                            IDRequester = choice
                        });
                    }
                    Console.WriteLine("Request sent successfully.\n");
                    break;

                }
                catch (AggregateException ex)
                {
                    Console.WriteLine("aaaaaaaaaaa");
                    Console.WriteLine(ex.InnerException.Message + "\n");
                }
                catch (UserInFriendsException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static void WriteDeleteFriendsMenu(int idUser, IMediator mediator)
        {
            List<Friends> friends = mediator.Send(new GetAllFriendsOfUserQuery
            {
                IDUser = idUser
            }).Result;

            foreach (var friend in friends)
            {
                string displayName = mediator.Send(new GetUserByIdQuery
                {
                    Id = friend.IDFriend
                }).Result.DisplayName;

                Console.WriteLine($"{displayName} - {friend.IDFriend}");
            }

            Console.Write("\nIntroduce the ID of the person you want to delete: ");
        }

        public static async void DeleteFriendMenu(int idUser, IMediator mediator)
        {
            int choice;
            string? choiceString;

            while (true)
            {

                WriteDeleteFriendsMenu(idUser, mediator);
                
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
                        IdUser = idUser,
                        IdFriend = choice
                    });

                    if (command.IsFaulted) throw command.Exception.InnerException;

                    // introduced id doesn't belong to any user in friends list

                    throw new UserNotFoundException(choice);
                }
                catch (UserInFriendsException)
                {
                    await mediator.Send(new RemoveFriendCommand
                    {
                        IDUser = idUser,
                        IDFriend = choice
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
        
        public static void WriteFriendsMenu(List<Friends> friends, IMediator mediator)
        {
            Console.WriteLine("Friends:\n");

            for (int i = 0; i < friends.Count; i++)
            {
                string displayName = mediator.Send(new GetUserByIdQuery
                {
                    Id = friends[i].IDFriend
                }).Result.DisplayName;

                Console.WriteLine($"{i + 1}. {displayName}");
            }
            Console.Write("\nPick someone to send a message to: ");
        }

        public static void FriendsMenu(int idUser, IMediator mediator)
        {
            int choice;
            string? choiceString;

            List<Friends> friends = mediator.Send(new GetAllFriendsOfUserQuery
            {
                IDUser = idUser
            }).Result;

            while (true)
            {
                WriteFriendsMenu(friends, mediator);

                choiceString = Console.ReadLine();

                Console.Clear();

                if (choiceString == "back") break;

                choice = ConvertInputToInt(choiceString);
                try
                {
                    int numberOfFriends = friends.Count;

                    if (CheckIfChoiceValid(choice, numberOfFriends))
                    {
                        MessagesMenu(idUser, friends[choice - 1].IDFriend, mediator);
                    }
                }
                catch (NumberBetweenException ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        public static void WriteMessagesBetweenTwoUsers(int idUser, int idFriend, List<Message> messages)
        {
            foreach (Message message in messages)
            {
                if (message.IDSender == idUser && message.IDReceiver == idFriend)
                {
                    Console.WriteLine(string.Format("{0,70}", message.Text));
                }
                else if (message.IDSender == idFriend && message.IDReceiver == idUser)
                {
                    Console.WriteLine(message.Text);
                }
            }
            Console.Write("\n>: ");
        }

        public static async void MessagesMenu(int idUser, int idFriend, IMediator mediator)
        {
            string? sentMessage;

            while (true)
            {
                List<Message> messages = mediator.Send(new GetMessagesBetweenTwoUsersQuery
                {
                    idUser1 = idUser,
                    idUser2 = idFriend
                }).Result;

                WriteMessagesBetweenTwoUsers(idUser, idFriend, messages);

                sentMessage = Console.ReadLine();

                Console.Clear();

                if (sentMessage == "back") break;

                try
                {
                    Task<MediatR.Unit> command = mediator.Send(new CheckIfMessageValid { Message = sentMessage });

                    if (command.IsFaulted) throw command.Exception;

                    await mediator.Send(new AddMessageCommand
                    {
                        IDSender = idUser,
                        IDReceiver = idFriend,
                        Text = sentMessage
                    });
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

            // add data to database
            
            /*context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Add(usersData[0]);
            context.Add(usersData[1]);
            context.Add(usersData[2]);

            context.SaveChanges();*/

            /*var users = context.Users
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
            }*/
            
            var services = new ServiceCollection()
                .AddScoped<IUserRepository, InMemoryUserRepository>()
                .AddScoped<IMessageRepository, InMemoryMessageRepository>()
                .AddScoped<IAppDbContext, AppDbContext>()
                .AddMediatR(typeof(IUserRepository))
                .BuildServiceProvider();

            // copied this from the website

/*            services.AddDbContext<AppContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
*/
            var mediator = services.GetRequiredService<IMediator>();

            /*mediator.Send(new AddUsers
            {
                Users = users
            });

            messages = messages.OrderBy(x => x.DateTime).ToList();

            mediator.Send(new AddMessages
            {
                Messages = messages
            });*/
            
            FirstMenu(mediator);

            return 0;
        }
    }
}