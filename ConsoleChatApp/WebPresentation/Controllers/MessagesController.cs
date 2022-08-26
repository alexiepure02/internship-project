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

        // add DataAnnotations

        public MessagesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessagePutPostDto product)
        {
            var command = _mapper.Map<CreateMessageCommand>(product);

            var created = await _mediator.Send(command);
            var dto = _mapper.Map<MessageGetDto>(created);

            return CreatedAtAction(nameof(GetMessages), new { messageId = created.ID }, dto);
        }

        [HttpGet]
        [Route("{idLogged}/friends/{idFriend}")]
        public async Task<IActionResult> GetMessages(int idLogged, int idFriend)
        {
            var query = new GetMessagesBetweenTwoUsersQuery { IDUser1 = idLogged, IDUser2 = idFriend };
            var result = await _mediator.Send(query);
            var mappedResult = _mapper.Map<List<MessageGetDto>>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetMessageByIdQuery { ID = id };
            var result = await _mediator.Send(query);
            var mappedResult = _mapper.Map<MessageGetDto>(result);
            return Ok(mappedResult);
        }
    }
}
