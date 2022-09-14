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
    [Route("api/friend-requests")]
    [ApiController]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<FriendRequestsController> _logger;

        public FriendRequestsController(IMediator mediator, IMapper mapper, ILogger<FriendRequestsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest([FromBody] FriendRequestPutPostDto friendRequest)
        {
            _logger.LogInformation("Creating create friend request command... ");
            var command = new CreateFriendRequestCommand
            {
                IDUser = friendRequest.IDUser,
                IDRequester = friendRequest.IDRequester
            };

            _logger.LogInformation("Calling create friend request command using mediator... ");
            var result = await _mediator.Send(command);

            _logger.LogInformation("Mapping result object to Dto objecct... ");
            var mappedResult = _mapper.Map<FriendRequestGetDto>(result);

            return CreatedAtAction(nameof(GetById), new { id = mappedResult.ID }, mappedResult);
        }

        [HttpGet]
        [Route("{idLogged}")]
        public async Task<IActionResult> GetAllFriendRequestsOfUser(int idLogged)
        {
            _logger.LogInformation("Creating get all friend requests of user query... ");
            var query = new GetAllFriendRequestsOfUserQuery { IDUser = idLogged };

            _logger.LogInformation("Calling get all friend requests of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<FriendRequestGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Creating get friend request by id query... ");
            var query = new GetFriendRequestByIdQuery { ID = id };

            _logger.LogInformation("Calling get friend request by id query using mediator... ");
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogInformation($"Friend request with the id = {id} not found.");
                return NotFound();
            }

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<FriendRequestGetDto>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{idLogged},{idRequester}")]
        public async Task<IActionResult> GetFriendRequestOfUser(int idLogged, int idRequester)
        {
            _logger.LogInformation("Creating get friend request of user query... ");
            var query = new GetFriendRequestOfUserQuery { IDUser = idLogged, IDRequester = idRequester };

            _logger.LogInformation("Calling get friend request of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<FriendRequestGetDto>(result);

            return Ok(mappedResult);
        }

        [HttpPut]
        [Route("{idLogged},{idRequester},{accepted}")]
        public IActionResult UpdateFriendRequest(int idLogged, int idRequester, bool accepted)
        {
            _logger.LogInformation("Creating update friend request command... ");
            var command = new UpdateFriendRequestCommand
            {
                IDUser = idLogged,
                IDRequester = idRequester,
                Accepted = accepted,

            };

            _logger.LogInformation("Calling update friend request command using mediator... ");
            var result = _mediator.Send(command);

            if (result.IsCompleted == false)
            {
                _logger.LogInformation($"Command not completed.");
                return NotFound();
            }

            return NoContent();
        }
    }
}
