using Application.Commands.CreateFriendRequestCommand;
using Application.Commands.UpdateFriendRequestCommand;
using Application.Queries.GetAllFriendRequestsOfUserQuery;
using Application.Queries.GetFriendRequestByIdQuery;
using Application.Queries.GetFriendRequestOfUserQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Dto;

namespace WebPresentation.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        // add DataAnnotations

        public FriendRequestsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest([FromBody] FriendRequestPutPostDto friendRequest)
        {
            var command = new CreateFriendRequestCommand
            {
                IDUser = friendRequest.IDUser,
                IDRequester = friendRequest.IDRequester
            };

            var result = await _mediator.Send(command);
            var mappedResult = _mapper.Map<FriendRequestGetDto>(result);

            if (mappedResult.ID == 0)
                return BadRequest(mappedResult);
            else
                return CreatedAtAction(nameof(GetById), new { id = mappedResult.ID }, mappedResult);
        }

        [HttpGet]
        [Route("{idUser}/getAllFriendRequests")]
        public async Task<IActionResult> GetAllFriendRequestsOfUser(int idUser)
        {
            var query = new GetAllFriendRequestsOfUserQuery { IDUser = idUser };
            var result = await _mediator.Send(query);
            var mappedResult = _mapper.Map<List<FriendRequestGetDto>>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetFriendRequestByIdQuery { ID = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            var mappedResult = _mapper.Map<FriendRequestGetDto>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{idLogged}/friends/{idRequester}")]
        public async Task<IActionResult> GetFriendRequestOfUser(int idLogged, int idRequester)
        {
            var query = new GetFriendRequestOfUserQuery { IDUser = idLogged, IDRequester = idRequester };
            var result = await _mediator.Send(query);
            var mappedResult = _mapper.Map<FriendRequestGetDto>(result);
            return Ok(mappedResult);
        }

        [HttpPut]
        [Route("accepted")]
        public IActionResult UpdateFriendRequest(bool accepted, [FromBody] FriendRequestPutPostDto updated)
        {
            var command = new UpdateFriendRequestCommand
            {
                IDUser = updated.IDUser,
                IDRequester = updated.IDRequester,
                Accepted = accepted,

            };
            var result = _mediator.Send(command);

            if (result == null)
                return NotFound();

            return NoContent();
        }
    }
}
