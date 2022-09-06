using Application.Commands.CreateMessageCommand;
using Application.Queries.GetMessageByIdQuery;
using Application.Queries.GetMessagesBetweenTwoUsersQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Dto;

namespace WebPresentation.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMapper mapper, IMediator mediator, ILogger<MessagesController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessagePutPostDto product)
        {
            _logger.LogInformation("Creating create message command using data from body... ");
            var command = new CreateMessageCommand
            {
                IDSender = product.IDSender,
                IDReceiver = product.IDReceiver,
                Text = product.Text
            };
            _logger.LogInformation("Calling create message command using mediator... ");
            var result = await _mediator.Send(command);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<MessageGetDto>(result);

            return CreatedAtAction(nameof(GetById), new { id = mappedResult.ID }, mappedResult);
        }

        [HttpGet]
        [Route("{idLogged}/friends/{idFriend}")]
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
    }
}
