using Application;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Domain.Exceptions;
using Domain;
using Application.Queries.GetUserByAccountQuery;
using Application.Queries.GetAllUsersQuery;
using Application.Commands.CreateUserCommand;
using Application.Queries.GetUserByIdQuery;
using Application.Commands.UpdateFriendRequestCommand;
using Application.Commands.CreateFriendRequestCommand;
using Application.Commands.DeleteFriendCommand;
using Application.Commands.CreateMessageCommand;
using Application.Queries.GetAllFriendsOfUserQuery;
using Application.Queries.GetAllFriendRequestsOfUserQuery;
using Application.Queries.GetMessagesBetweenTwoUsersQuery;
using Microsoft.EntityFrameworkCore;
using Application.Queries.GetFriendRequestOfUserQuery;

namespace Presentation
{
    public class Program
    {

        private static IMediator Init()
        {
            var services = new ServiceCollection()
                .AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(@"Data Source=IEPURE\SQLEXPRESS;DataBase=InternshipProjectDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                .AddMediatR(typeof(IUserRepository))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .BuildServiceProvider();

            return services.GetRequiredService<IMediator>();
        }

        static async Task<int> Main(string[] args)
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
            
            var mediator = Init();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Friends list");
                Console.WriteLine("3. Friend requests list");
                Console.WriteLine("4. Send friend request");
                Console.WriteLine("5. Update friend request");
                Console.WriteLine("6. Delete friend");
                Console.WriteLine("7. Messages with a friend");
                Console.WriteLine("8. Send message to friend");
                Console.WriteLine();
                Console.Write("Insert action: ");
                var action = Convert.ToInt32(Console.ReadLine());

                Console.Clear();

                switch(action)
                {
                    case 1:
                        var loggedUser = await Login(mediator);
                        DisplayUser(loggedUser);
                        break;
                    case 2:
                        var friendsList = await GetFriendsOfUser(mediator);
                        DisplayFriendsList(friendsList);
                        break;
                    case 3:
                        var friendRequestsList = await GetFriendRequestsOfUser(mediator);
                        DisplayFriendRequestsList(friendRequestsList);
                        break;
                    case 4:
                        var sentRequest = await SendFriendRequest(mediator);
                        DisplayFriendRequest(sentRequest);
                        break;
                    case 5:
                        var friendRequest = await UpdateFriendRequest(mediator);
                        DisplayFriendRequest(friendRequest);
                        break;
                    case 6:
                        var deletedFriend = await DeleteFriend(mediator);
                        DisplayFriend(deletedFriend);
                        break;
                    case 7:
                        Console.WriteLine("Insert idUser1");
                        int idUser1 = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Insert idUser2");
                        int idUser2 = Convert.ToInt32(Console.ReadLine());
                        var messages = await GetMessages(mediator, idUser1, idUser2);
                        DisplayMessages(messages, idUser1, idUser2);
                        break;
                    case 8:
                        var createdMessage = await CreateMessage(mediator);
                        DisplayMessage(createdMessage);
                        break;
                    default:
                        Console.WriteLine($"Invalid action: {action}");
                        break;
                }
            }

            return 0;
        }

        public static async Task<User> Login(IMediator mediator)
        {
            var command = new GetUserByAccountQuery();
            Console.WriteLine($"Insert {nameof(command.Username)}");
            command.Username = Console.ReadLine();
            Console.WriteLine($"Insert {nameof(command.Password)}");
            command.Password = Console.ReadLine();

            return await mediator.Send(command);
        }
        public static void DisplayUser(User user)
        {
            Console.WriteLine("\nUser:\n");
            Console.WriteLine($"ID: {user.ID}");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Password: {user.Password}");
            Console.WriteLine($"DisplayName: {user.DisplayName}");
        }

        public static async Task<List<Friends>> GetFriendsOfUser(IMediator mediator)
        {
            var command = new GetAllFriendsOfUserQuery();
            Console.WriteLine($"Insert {nameof(command.IDUser)}");
            command.IDUser = Convert.ToInt32(Console.ReadLine());

            return await mediator.Send(command);
        }
        public static void DisplayFriendsList(List<Friends> friends)
        {
            Console.WriteLine("\nFriends:\n");
            foreach (var friend in friends)
            {
                Console.WriteLine($"{friend.IDUser} - {friend.IDFriend}");
            }
        }

        public static async Task<List<FriendRequests>>GetFriendRequestsOfUser(IMediator mediator)
        {
            var command = new GetAllFriendRequestsOfUserQuery();
            Console.WriteLine($"Insert {nameof(command.IDUser)}");
            command.IDUser = Convert.ToInt32(Console.ReadLine());

            return await mediator.Send(command);
        }
        public static void DisplayFriendRequestsList(List<FriendRequests> FriendRequests)
        {
            Console.WriteLine("\nFriend requests:\n");
            foreach (var friendRequest in FriendRequests)
            {
                Console.WriteLine($"{friendRequest.IDUser} - {friendRequest.IDRequester}");
            }
        }
    
        public static async Task<FriendRequests> SendFriendRequest(IMediator mediator)
        {
            var commandFriends = new GetAllFriendsOfUserQuery();
            Console.WriteLine($"Insert {nameof(commandFriends.IDUser)}");
            int idUser = Convert.ToInt32(Console.ReadLine());
            commandFriends.IDUser = idUser;

            var friends = await mediator.Send(commandFriends);
            DisplayFriendsList(friends);

            Console.WriteLine();

            var command = new CreateFriendRequestCommand();
            command.IDRequester = idUser;
            Console.WriteLine($"Insert id of future friend (not from list)");
            command.IDUser = Convert.ToInt32(Console.ReadLine());

            return await mediator.Send(command);
        }
        public static void DisplayFriendRequest(FriendRequests friendRequest)
        {
            Console.WriteLine("\nFriend request:\n");
            Console.WriteLine($"{friendRequest.IDUser} - {friendRequest.IDRequester}");
        }

        public static async Task<FriendRequests> UpdateFriendRequest(IMediator mediator)
        {
            var commandRequests = new GetAllFriendRequestsOfUserQuery();
            Console.WriteLine($"Insert {nameof(commandRequests.IDUser)}");
            int idUser;
            idUser = Convert.ToInt32(Console.ReadLine());
            commandRequests.IDUser = idUser;

            var friendRequestsList1 = await mediator.Send(commandRequests);
            DisplayFriendRequestsList(friendRequestsList1);

            Console.WriteLine();

            var command = new UpdateFriendRequestCommand();
            Console.WriteLine("Insert id (put - in front to decline)");
            int id = Convert.ToInt32(Console.ReadLine());

            if (id > 0)
            {
                command.IDUser = idUser;
                command.IDRequester = id;
                command.Accepted = true;
            }
            else
            {
                command.IDUser = idUser;
                command.IDRequester = Math.Abs(id);
                command.Accepted = false;
            }

            return await mediator.Send(command);
        }

        public static async Task<Friends> DeleteFriend(IMediator mediator)
        {

            var commandFriends = new GetAllFriendsOfUserQuery();
            Console.WriteLine($"Insert {nameof(commandFriends.IDUser)}");
            int idUser = Convert.ToInt32(Console.ReadLine());
            commandFriends.IDUser = idUser;

            var friends = await mediator.Send(commandFriends);
            DisplayFriendsList(friends);

            Console.WriteLine();

            var command = new DeleteFriendCommand();
            command.IDUser = idUser;
            Console.WriteLine($"Insert {nameof(command.IDFriend)}");
            command.IDFriend = Convert.ToInt32(Console.ReadLine());

            return await mediator.Send(command);
        }
        public static void DisplayFriend(Friends friend)
        {
            Console.WriteLine("\nFriend:\n");
            Console.WriteLine($"{friend.IDUser} - {friend.IDFriend}");
        }

        public static async Task<List<Message>> GetMessages(IMediator mediator, int idUser1, int idUser2)
        {
            var command = new GetMessagesBetweenTwoUsersQuery()
            {
                IDUser1 = idUser1,
                IDUser2 = idUser2
            };

            return await mediator.Send(command);
        }
        public static void DisplayMessages(List<Message> messages, int idUser1, int idUser2)
        {
            Console.WriteLine($"\nMessages between {idUser1} and {idUser2}\n");
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
        }

        public static async Task<Message> CreateMessage(IMediator mediator)
        {
            var command = new CreateMessageCommand();
            Console.WriteLine($"Insert {nameof(command.IDSender)}");
            command.IDSender = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Insert {nameof(command.IDReceiver)}");
            command.IDReceiver = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Insert {nameof(command.Text)}");
            command.Text = Console.ReadLine();

            return await mediator.Send(command);
        }
        public static void DisplayMessage(Message message)
        {
            Console.WriteLine("\nMessage:\n");
            Console.WriteLine($"ID Sender: {message.IDSender}");
            Console.WriteLine($"ID Receiver: {message.IDReceiver}");
            Console.WriteLine($"Text: {message.Text}");
            Console.WriteLine($"DateTime: {message.DateTime}");
        }
    }
}