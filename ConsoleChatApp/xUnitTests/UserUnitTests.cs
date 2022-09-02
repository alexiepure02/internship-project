using Application.Queries.GetAllUsersQuery;
using Application.Queries.GetUserByAccountQuery;
using Application.Queries.GetUserByIdQuery;
using AutoMapper;
using Domain;
using Domain.Exceptions;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        [Fact]
        public async Task Get_All_Users_GetAllUsersQueryIsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            await controller.GetUsers();

            _mediator.Verify(x => x.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Get_User_By_Id_GetUserByIdQueryIsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            await controller.GetById(1);

            _mediator.Verify(x => x.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Get_User_By_Id_GetUserByIdQueryWithCorrectUserIdIsCalled()
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

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            await controller.GetById(1);

            Assert.Equal(1, userId);
        }

        [Fact]
        public async Task Get_User_By_Id_ShouldReturnOkStatusCode()
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

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            var result = await controller.GetById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_User_By_Id_ShouldReturnFoundUser()
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
                .Returns((User src) => new UserGetDto
                {
                    ID = src.ID,
                    Username = src.Username,
                    Password = src.Password,
                    DisplayName = src.DisplayName
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            var result = await controller.GetById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal(userDto, okResult.Value);
        }

        [Fact]
        public async Task Get_User_By_Account_GetUserByAccountQueryIsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            await controller.GetByAccount("dummyUsername", "dummyPassword");

            _mediator.Verify(x => x.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Get_User_By_Account_GetUserByAccountQueryWithCorrectUserIdIsCalled()
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

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            await controller.GetByAccount("dummyUsername", "dummyPassword");

            Assert.Equal("dummyUsername", username);
            Assert.Equal("dummyPassword", password);

        }

        [Fact]
        public async Task Get_User_By_Account_ShouldReturnOkStatusCode()
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

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            var result = await controller.GetByAccount("dummyUsername", "dummyPassword");

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_User_By_Account_ShouldReturnFoundUser()
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
                .Returns((User src) => new UserGetDto
                {
                    ID = src.ID,
                    Username = src.Username,
                    Password = src.Password,
                    DisplayName = src.DisplayName
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetUserByAccountQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var controller = new UsersController(_mapper.Object, _mediator.Object);
            var result = await controller.GetByAccount("dummyUsername", "dummyPassword");

            var okResult = result as OkObjectResult;

            Assert.Equal(userDto, okResult.Value);
        }
    }
}