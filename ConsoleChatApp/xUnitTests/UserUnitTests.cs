/*using Application.Commands.CreateFriendRequestCommand;
using Application.Queries.GetAllFriendRequestsOfUserQuery;
using Application.Queries.GetAllFriendsOfUserQuery;
using Application.Queries.GetAllUsersQuery;
using Application.Queries.GetFriendOfUserQuery;
using Application.Queries.GetFriendRequestByIdQuery;
using Application.Queries.GetFriendRequestOfUserQuery;
using Application.Queries.GetUserByAccountQuery;
using Application.Queries.GetUserByIdQuery;
using AutoMapper;
using Castle.Core.Logging;
using Domain;
using Domain.Exceptions;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using WebPresentation.Controllers;
using WebPresentation.Dto;

namespace xUnitTests
{
    public class UserUnitTests
    {
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ILogger<UsersController>> _logger = new Mock<ILogger<UsersController>>();

        [Fact]
        public async Task GetAllUsersQuery_IsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetUsers();

            _mediator.Verify(x => x.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetUserByIdQuery_IsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetById(1);

            _mediator.Verify(x => x.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetUserByIdQuery_WithCorrectUserIdIsCalled()
        {
            int userId = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetUserByIdQuery, CancellationToken>(async (q, c) =>
                {
                    userId = q.IDUser;
                    return await Task.FromResult(
                        new User
                        {
                            ID = q.IDUser,
                            Username = "dummyUsername",
                            Password = "dummyPassword",
                            DisplayName = "Dummy"
                        });
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetById(1);

            Assert.Equal(1, userId);
        }

        [Fact]
        public async Task GetUserByIdQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new User
                    {
                        ID = 1,
                        Username = "dummyUsername",
                        Password = "dummyPassword",
                        DisplayName = "Dummy"
                    });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetUserByIdQuery_ShouldReturnFoundUser()
        {
            var user = new User
            {
                ID = 1,
                Username = "dummyUsername",
                Password = "dummyPassword",
                DisplayName = "Dummy"
            };

            var userDto = new UserGetDto
            {
                ID = user.ID,
                Username = user.Username,
                Password = user.Password,
                DisplayName = user.DisplayName
            };

            _mapper
                .Setup(m => m.Map<UserGetDto>(It.IsAny<User>()))
                .Returns(userDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal(userDto, okResult.Value);
        }

        [Fact]
        public async Task GetUserByAccountQuery_IsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetByAccount("dummyUsername", "dummyPassword");

            _mediator.Verify(x => x.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetUserByAccountQuery_WithCorrectUserIdIsCalled()
        {
            string username = "";
            string password = "";

            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetUserByAccountQuery, CancellationToken>(async (q, c) =>
                {
                    username = q.Username;
                    password = q.Password;
                    return await Task.FromResult(
                        new User
                        {
                            ID = 1,
                            Username = q.Username,
                            Password = q.Password,
                            DisplayName = "Dummy"
                        });
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetByAccount("dummyUsername", "dummyPassword");

            Assert.Equal("dummyUsername", username);
            Assert.Equal("dummyPassword", password);

        }

        [Fact]
        public async Task GetUserByAccountQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new User
                    {
                        ID = 1,
                        Username = "dummyUsername",
                        Password = "dummyPassword",
                        DisplayName = "Dummy"
                    });

            _mapper
                .Setup(m => m.Map<UserGetDto>(It.IsAny<User>()))
                .Returns((User src) => new UserGetDto
                {
                    ID = src.ID,
                    Username = src.Username,
                    Password = src.Password,
                    DisplayName = src.DisplayName
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetByAccount("dummyUsername", "dummyPassword");

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetUserByAccountQuery_ShouldReturnFoundUser()
        {
            var user = new User
            {
                ID = 1,
                Username = "dummyUsername",
                Password = "dummyPassword",
                DisplayName = "Dummy"
            };

            var userDto = new UserGetDto
            {
                ID = user.ID,
                Username = user.Username,
                Password = user.Password,
                DisplayName = user.DisplayName
            };

            _mapper
                .Setup(m => m.Map<UserGetDto>(It.IsAny<User>()))
                .Returns(userDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetByAccount("dummyUsername", "dummyPassword");

            var okResult = result as OkObjectResult;

            Assert.Equal(userDto, okResult.Value);
        }

        [Fact]
        public async Task GetAllFriendsOfUserQuery_IsCalled()
        {
            _mediator
            .Setup(m => m.Send(It.IsAny<GetAllFriendsOfUserQuery>(), It.IsAny<CancellationToken>()))
            .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetAllFriendsOfUser(1);

            _mediator.Verify(x => x.Send(It.IsAny<GetAllFriendsOfUserQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetAllFriendsOfUserQuery_WithCorrectUserIdIsCalled()
        {
            int userId = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllFriendsOfUserQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetAllFriendsOfUserQuery, CancellationToken>(async (q, c) =>
                {
                    userId = q.IDUser;
                    return await Task.FromResult(
                        new List<Friends>
                        {
                            new Friends
                            {
                                IDUser = 1,
                                IDFriend = 2
                            },
                            new Friends
                            {
                                IDUser = 1,
                                IDFriend = 3
                            }
                        });
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetAllFriendsOfUser(1);

            Assert.Equal(1, userId);
        }

        [Fact]
        public async Task GetAllFriendsOfUserQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllFriendsOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Friends>
                    {
                        new Friends
                        {
                            IDUser = 1,
                            IDFriend = 2
                        },
                        new Friends
                        {
                            IDUser = 1,
                            IDFriend = 3
                        }
                    });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetAllFriendsOfUser(1);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllFriendsOfUserQuery_ShouldReturnFoundFriends()
        {
            var friends = new List<Friends>
            {
                new Friends
                {
                    IDUser = 1,
                    IDFriend = 2
                },
                new Friends
                {
                    IDUser = 1,
                    IDFriend = 3
                }
            };

            var friendsDto = new List<FriendGetDto>
            {
                new FriendGetDto
                {
                    IDUser = friends[0].IDUser,
                    IDFriend = friends[0].IDFriend
                },
                new FriendGetDto
                {
                    IDUser = friends[1].IDUser,
                    IDFriend = friends[1].IDFriend
                }
            };

            _mapper
                .Setup(m => m.Map<List<FriendGetDto>>(It.IsAny<List<Friends>>()))
                .Returns(friendsDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllFriendsOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(friends);

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetAllFriendsOfUser(1);

            var okResult = result as OkObjectResult;

            Assert.Equal(friendsDto, okResult.Value);
        }

        [Fact]
        public async Task GetFriendOfUserQuery_IsCalled()
        {
            _mediator
            .Setup(m => m.Send(It.IsAny<GetFriendOfUserQuery>(), It.IsAny<CancellationToken>()))
            .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetFriendOfUser(1, 2);

            _mediator.Verify(x => x.Send(It.IsAny<GetFriendOfUserQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetFriendOfUserQuery_WithCorrectUserIdAndFriendIdIsCalled()
        {
            int userId = 0;
            int friendId = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendOfUserQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetFriendOfUserQuery, CancellationToken>(async (q, c) =>
                {
                    userId = q.IDUser;
                    friendId = q.IDFriend;
                    return await Task.FromResult(
                        new Friends
                        {
                            IDUser = 1,
                            IDFriend = 2
                        });
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetFriendOfUser(1, 2);

            Assert.Equal(1, userId);
            Assert.Equal(2, friendId);
        }

        [Fact]
        public async Task GetFriendOfUserQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new Friends
                {
                    IDUser = 1,
                    IDFriend = 2
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetFriendOfUser(1, 2);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetFriendOfUserQuery_ShouldReturnFoundFriend()
        {
            var friend = new Friends
            {
                IDUser = 1,
                IDFriend = 2
            };

            var friendDto = new FriendGetDto
            {
                IDUser = friend.IDUser,
                IDFriend = friend.IDFriend
            };

            _mapper
                .Setup(m => m.Map<FriendGetDto>(It.IsAny<Friends>()))
                .Returns(friendDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(friend);

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetFriendOfUser(1, 2);

            var okResult = result as OkObjectResult;

            Assert.Equal(friendDto, okResult.Value);
        }

        [Fact]
        public async Task GetAllFriendRequestsOfUserQuery_IsCalled()
        {
            _mediator
            .Setup(m => m.Send(It.IsAny<GetAllFriendRequestsOfUserQuery>(), It.IsAny<CancellationToken>()))
            .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetAllFriendRequestsOfUser(1);

            _mediator.Verify(x => x.Send(It.IsAny<GetAllFriendRequestsOfUserQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetAllFriendRequestsOfUserQuery_WithCorrectUserIdIsCalled()
        {
            int userId = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllFriendRequestsOfUserQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetAllFriendRequestsOfUserQuery, CancellationToken>(async (q, c) =>
                {
                    userId = q.IDUser;
                    return await Task.FromResult(
                        new List<FriendRequests>
                        {
                            new FriendRequests
                            {
                                IDUser = 1,
                                IDRequester = 2
                            },
                            new FriendRequests
                            {
                                IDUser = 1,
                                IDRequester = 3
                            }
                        });
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetAllFriendRequestsOfUser(1);

            Assert.Equal(1, userId);
        }

        [Fact]
        public async Task GetAllFriendRequestsOfUserQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllFriendRequestsOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<FriendRequests>
                        {
                            new FriendRequests
                            {
                                IDUser = 1,
                                IDRequester = 2
                            },
                            new FriendRequests
                            {
                                IDUser = 1,
                                IDRequester = 3
                            }
                        });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetAllFriendRequestsOfUser(1);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllFriendRequestsOfUserQuery_ShouldReturnFoundFriendRequests()
        {
            var friendRequests = new List<FriendRequests>
                        {
                            new FriendRequests
                            {
                                IDUser = 1,
                                IDRequester = 2
                            },
                            new FriendRequests
                            {
                                IDUser = 1,
                                IDRequester = 3
                            }
                        };

            var friendRequestsDto = new List<FriendRequestGetDto>
                            {
                            new FriendRequestGetDto
                            {
                                IDUser = friendRequests[0].IDUser,
                                IDRequester = friendRequests[0].IDRequester
                            },
                            new FriendRequestGetDto
                            {
                                IDUser = friendRequests[1].IDUser,
                                IDRequester = friendRequests[1].IDRequester
                            }
                        };

            _mapper
                .Setup(m => m.Map<List<FriendRequestGetDto>>(It.IsAny<List<FriendRequests>>()))
                .Returns(friendRequestsDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllFriendRequestsOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(friendRequests);

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetAllFriendRequestsOfUser(1);

            var okResult = result as OkObjectResult;

            Assert.Equal(friendRequestsDto, okResult.Value);
        }

        [Fact]
        public async Task GetFriendRequestByIdQuery_IsCalled()
        {
            _mediator
            .Setup(m => m.Send(It.IsAny<GetFriendRequestByIdQuery>(), It.IsAny<CancellationToken>()))
            .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetFriendRequestById(1);

            _mediator.Verify(x => x.Send(It.IsAny<GetFriendRequestByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetFriendRequestByIdQuery_WithCorrectUserIdIsCalled()
        {
            int id = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendRequestByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetFriendRequestByIdQuery, CancellationToken>(async (q, c) =>
                {
                    id = q.ID;
                    return await Task.FromResult(
                        new FriendRequests
                        {
                            ID = q.ID,
                            IDUser = 0,
                            IDRequester = 0
                        });
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetFriendRequestById(1);

            Assert.Equal(1, id);
        }

        [Fact]
        public async Task GetFriendRequestByIdQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendRequestByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new FriendRequests
                {
                    ID = 1,
                    IDUser = 0,
                    IDRequester = 0
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetFriendRequestById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetFriendRequestByIdQuery_ShouldReturnFoundFriendRequests()
        {
            var friendRequest = new FriendRequests
            {
                ID = 1,
                IDUser = 0,
                IDRequester = 0
            };

            var friendRequestDto = new FriendRequestGetDto
            {
                ID = friendRequest.ID,
                IDUser = friendRequest.IDUser,
                IDRequester = friendRequest.IDRequester
            };

            _mapper
                .Setup(m => m.Map<FriendRequestGetDto>(It.IsAny<FriendRequests>()))
                .Returns(friendRequestDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendRequestByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(friendRequest);

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetFriendRequestById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal(friendRequestDto, okResult.Value);
        }

        [Fact]
        public async Task GetFriendRequestOfUserQuery_IsCalled()
        {
            _mediator
            .Setup(m => m.Send(It.IsAny<GetFriendRequestOfUserQuery>(), It.IsAny<CancellationToken>()))
            .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetFriendRequestOfUser(1, 2);

            _mediator.Verify(x => x.Send(It.IsAny<GetFriendRequestOfUserQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetFriendRequestOfUserQuery_WithCorrectUserIdIsCalled()
        {
            int idUser = 0;
            int idRequester = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendRequestOfUserQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetFriendRequestOfUserQuery, CancellationToken>(async (q, c) =>
                {
                    idUser = q.IDUser;
                    idRequester = q.IDRequester;
                    return await Task.FromResult(
                        new FriendRequests
                        {
                            IDUser = 1,
                            IDRequester = 2
                        });
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetFriendRequestOfUser(1, 2);

            Assert.Equal(1, idUser);
            Assert.Equal(2, idRequester);
        }

        [Fact]
        public async Task GetFriendRequestOfUserQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendRequestOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new FriendRequests
                {
                    IDUser = 1,
                    IDRequester = 2
                });

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetFriendRequestOfUser(1,2);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetFriendRequestOfUserQuery_ShouldReturnFoundFriendRequests()
        {
            var friendRequest = new FriendRequests
            {
                IDUser = 1,
                IDRequester = 2
            };

            var friendRequestDto = new FriendRequestGetDto
            {
                IDUser = friendRequest.IDUser,
                IDRequester = friendRequest.IDRequester
            };

            _mapper
                .Setup(m => m.Map<FriendRequestGetDto>(It.IsAny<FriendRequests>()))
                .Returns(friendRequestDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetFriendRequestOfUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(friendRequest);

            var controller = new UsersController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetFriendRequestOfUser(1, 2);

            var okResult = result as OkObjectResult;

            Assert.Equal(friendRequestDto, okResult.Value);
        }
    }
}*/