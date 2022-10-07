using Application.Commands.CreateMessageCommand;
using Application.Queries.GetMessageByIdQuery;
using Application.Queries.GetMessagesBetweenTwoUsersQuery;
using Application.Queries.GetNumberOfMessagesBetweenTwoUsersQuery;
using Application.Queries.GetSomeMessagesFromOffsetQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebPresentation.Dto;
using WebPresentation.SignalR;

namespace WebPresentation.Controllers
{
    //[Authorize]
    [EnableCors("ClientPermission")]
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<MessagesController> _logger;
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;
        private readonly IHttpContextAccessor _contextAccessor;

        public MessagesController(IMapper mapper, IMediator mediator, ILogger<MessagesController> logger, IHubContext<ChatHub, IChatClient> chatHub, IHttpContextAccessor contextAccessor)
        {
            _chatHub = chatHub;
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        [Route("msg")]
        [HttpPost]
        public async Task<IActionResult> Post(MessagePutPostDto message)
        {
            _logger.LogInformation("Creating create message command using data from body... ");
            var command = new CreateMessageCommand
            {
                IDSender = message.IDSender,
                IDReceiver = message.IDReceiver,
                Text = message.Text
            };
            _logger.LogInformation("Calling create message command using mediator... ");
            var result = await _mediator.Send(command);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<MessageGetDto>(result);


            var connectionId = _contextAccessor.HttpContext?.Request.Headers["x-signalr-connection"] ?? "";

            //await _chatHub.Clients.Others.ReceiveMessage(mappedResult);
            //await _chatHub.Clients.All.ReceiveMessage(mappedResult);
            await _chatHub.Clients.AllExcept(connectionId).ReceiveMessage(mappedResult);

            return CreatedAtAction(nameof(GetById), new { id = mappedResult.ID }, mappedResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessagePutPostDto message)
        {
            _logger.LogInformation("Creating create message command using data from body... ");
            var command = new CreateMessageCommand
            {
                IDSender = message.IDSender,
                IDReceiver = message.IDReceiver,
                Text = message.Text
            };
            _logger.LogInformation("Calling create message command using mediator... ");
            var result = await _mediator.Send(command);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<MessageGetDto>(result);

            return CreatedAtAction(nameof(GetById), new { id = mappedResult.ID }, mappedResult);
        }

        [HttpGet]
        [Route("{idLogged},{idFriend}")]
        public async Task<IActionResult> GetMessages(int idLogged, int idFriend)
        {
            _logger.LogInformation("Creating get messages between two users query... ");
            var query = new GetMessagesBetweenTwoUsersQuery { IDUser1 = idLogged, IDUser2 = idFriend };

            _logger.LogInformation("Calling get messages between two users query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<MessageGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Creating get message by id query... ");
            var query = new GetMessageByIdQuery { IDMessage = id };

            _logger.LogInformation("Calling get message by id query using mediator... ");
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogInformation($"Message with the id = {id} not found.");
                return NotFound();
            }

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<MessageGetDto>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{idLogged},{idFriend}/length")]
        public async Task<IActionResult> GetNumberOfMessages(int idLogged, int idFriend)
        {
            _logger.LogInformation("Creating get number of messages query... ");
            var query = new GetNumberOfMessagesBetweenTwoUsersQuery
            {
                IDUser1 = idLogged,
                IDUser2 = idFriend
            };

            _logger.LogInformation("Calling get number of messages query using mediator... ");
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("{idLogged},{idFriend}/{offset},{numberOfMessages}")]
        public async Task<IActionResult> GetSomeMessages(int idLogged, int idFriend, int offset, int numberOfMessages)
        {
            _logger.LogInformation("Creating get some messages query... ");
            var query = new GetSomeMessagesFromOffsetQuery
            {
                IDUser1 = idLogged,
                IDUser2 = idFriend,
                Offset = offset,
                NumberOfMessages = numberOfMessages
            };

            _logger.LogInformation("Calling get some messages query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<MessageGetDto>>(result);

            return Ok(mappedResult);
        }

    }
}
