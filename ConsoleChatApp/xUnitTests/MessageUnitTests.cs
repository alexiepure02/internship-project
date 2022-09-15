using Application.Queries.GetMessageByIdQuery;
using Application.Queries.GetMessagesBetweenTwoUsersQuery;
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
    public class MessageTests
    {
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ILogger<MessagesController>> _logger = new Mock<ILogger<MessagesController>>();

        [Fact]
        public async Task GetMessageBetweenTwoUsersQuery_IsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessagesBetweenTwoUsersQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetMessages(1, 2);

            _mediator.Verify(x => x.Send(It.IsAny<GetMessagesBetweenTwoUsersQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetMessageBetweenTwoUsersQuery_WithCorrectMessagesIsCalled()
        {
            int idUser1 = 0;
            int idUser2 = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessagesBetweenTwoUsersQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetMessagesBetweenTwoUsersQuery, CancellationToken>(async (q, c) =>
                {
                    idUser1 = q.IDUser1;
                    idUser2 = q.IDUser2;
                    return await Task.FromResult(
                        new List<Message>
                        {
                            new Message
                            {
                                ID = 1,
                                IDSender = q.IDUser1,
                                IDReceiver = q.IDUser2,
                                Text = "hello1.",
                                DateTime = "12"
                            },
                            new Message
                            {
                                ID = 2,
                                IDSender = q.IDUser2,
                                IDReceiver = q.IDUser1,
                                Text = "hello2.",
                                DateTime = "13"
                            },
                            new Message
                            {
                                ID = 3,
                                IDSender = q.IDUser1,
                                IDReceiver = q.IDUser2,
                                Text = "hello3.",
                                DateTime = "14"
                            },
                            new Message
                            {
                                ID = 4,
                                IDSender = q.IDUser1,
                                IDReceiver = q.IDUser2,
                                Text = "hello4.",
                                DateTime = "15"
                            }
                        });
                });

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetMessages(1, 2);

            Assert.Equal(1, idUser1);
            Assert.Equal(2, idUser2);
        }

        [Fact]
        public async Task GetMessagesBetweenTwoUsersQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessagesBetweenTwoUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new List<Message>
                        {
                            new Message
                            {
                                ID = 1,
                                IDSender = 1,
                                IDReceiver = 2,
                                Text = "hello1.",
                                DateTime = "12"
                            },
                            new Message
                            {
                                ID = 2,
                                IDSender = 2,
                                IDReceiver = 1,
                                Text = "hello2.",
                                DateTime = "13"
                            },
                            new Message
                            {
                                ID = 3,
                                IDSender = 1,
                                IDReceiver = 2,
                                Text = "hello3.",
                                DateTime = "14"
                            },
                            new Message
                            {
                                ID = 4,
                                IDSender = 1,
                                IDReceiver = 2,
                                Text = "hello4.",
                                DateTime = "15"
                            }
                        }
                );

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetMessages(1, 2);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetMessagesBetweenTwoUsersQuery_ShouldReturnFoundMessages()
        {
            var messages = new List<Message>
            {
                new Message
                {
                    ID = 1,
                    IDSender = 1,
                    IDReceiver = 2,
                    Text = "hello1.",
                    DateTime = "12"
                },
                new Message
                {
                    ID = 2,
                    IDSender = 2,
                    IDReceiver = 1,
                    Text = "hello2.",
                    DateTime = "13"
                },
                new Message
                {
                    ID = 3,
                    IDSender = 1,
                    IDReceiver = 2,
                    Text = "hello3.",
                    DateTime = "14"
                },
                new Message
                {
                    ID = 4,
                    IDSender = 1,
                    IDReceiver = 2,
                    Text = "hello4.",
                    DateTime = "15"
                }
            };
            var messagesDto = new List<MessageGetDto>
            {
                new MessageGetDto
                {
                    ID = messages[0].ID,
                    IDSender = messages[0].IDSender,
                    IDReceiver = messages[0].IDReceiver,
                    Text = messages[0].Text,
                    DateTime = messages[0].DateTime
                },
                new MessageGetDto
                {
                    ID = messages[1].ID,
                    IDSender = messages[1].IDSender,
                    IDReceiver = messages[1].IDReceiver,
                    Text = messages[1].Text,
                    DateTime = messages[1].DateTime
                },
                new MessageGetDto
                {
                    ID = messages[2].ID,
                    IDSender = messages[2].IDSender,
                    IDReceiver = messages[2].IDReceiver,
                    Text = messages[2].Text,
                    DateTime = messages[2].DateTime
                },
                new MessageGetDto
                {
                    ID = messages[3].ID,
                    IDSender = messages[3].IDSender,
                    IDReceiver = messages[3].IDReceiver,
                    Text = messages[3].Text,
                    DateTime = messages[3].DateTime
                }
            };

            _mapper
                .Setup(m => m.Map<List<MessageGetDto>>(It.IsAny<List<Message>>()))
                .Returns(messagesDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessagesBetweenTwoUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(messages);

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetMessages(1, 2);

            var okResult = result as OkObjectResult;

            Assert.Equal(messagesDto, okResult.Value);
        }

        [Fact]
        public async Task GetMessageByIdQuery_IsCalled()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessageByIdQuery>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetById(1);

            _mediator.Verify(x => x.Send(It.IsAny<GetMessageByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetMessageByIdQuery_WithCorrectMessagesIsCalled()
        {
            int idMessage = 0;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessageByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns<GetMessageByIdQuery, CancellationToken>(async (q, c) =>
                {
                    idMessage = q.IDMessage;
                    return await Task.FromResult(
                        new Message
                        {
                            ID = q.IDMessage,
                            IDSender = 1,
                            IDReceiver = 2,
                            Text = "hello1.",
                            DateTime = "12"

                        });
                });

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            await controller.GetById(1);

            Assert.Equal(1, idMessage);
        }

        [Fact]
        public async Task GetMessageByIdQuery_ShouldReturnOkStatusCode()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessageByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                            new Message
                            {
                                ID = 1,
                                IDSender = 1,
                                IDReceiver = 2,
                                Text = "hello1.",
                                DateTime = "12"
                            });

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetMessageByIdQuery_ShouldReturnFoundMessage()
        {
            var message = new Message
            {
                ID = 1,
                IDSender = 1,
                IDReceiver = 2,
                Text = "hello1.",
                DateTime = "12"
            };
            var messageDto = new MessageGetDto
            {
                ID = message.ID,
                IDSender = message.IDSender,
                IDReceiver = message.IDReceiver,
                Text = message.Text,
                DateTime = message.DateTime
            };

            _mapper
                .Setup(m => m.Map<MessageGetDto>(It.IsAny<Message>()))
                .Returns(messageDto);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetMessageByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(message);

            var controller = new MessagesController(_mapper.Object, _mediator.Object, _logger.Object);
            var result = await controller.GetById(1);

            var okResult = result as OkObjectResult;

            Assert.Equal(messageDto, okResult.Value);
        }
    }
}