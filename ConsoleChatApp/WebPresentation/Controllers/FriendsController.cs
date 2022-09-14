using Application.Commands.DeleteFriendCommand;
using Application.Queries.GetAllFriendsOfUserQuery;
using Application.Queries.GetFriendOfUserQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Dto;

namespace WebPresentation.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<FriendsController> _logger;

        public FriendsController(IMediator mediator, IMapper mapper, ILogger<FriendsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("{idLogged}")]
        public async Task<IActionResult> GetAllFriendsOfUser(int id)
        {
            _logger.LogInformation("Creating get all friends of user query... ");
            var query = new GetAllFriendsOfUserQuery { IDUser = id};

            _logger.LogInformation("Calling get all friends of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<FriendGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{idLogged},{idFriend}")]
        public async Task<IActionResult> GetFriendOfUser(int idLogged, int idFriend)
        {
            _logger.LogInformation("Creating get friend of user query... ");
            var query = new GetFriendOfUserQuery { IDUser = idLogged, IDFriend = idFriend };

            _logger.LogInformation("Calling get friend of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<FriendGetDto>(result);
            return Ok(mappedResult);
        }

        [HttpDelete]
        [Route("{idLogged},{idFriend}")]
        public async Task<IActionResult> DeleteFriend(int idLogged, int idFriend)
        {
            _logger.LogInformation("Creating delete friend command... ");
            var command = new DeleteFriendCommand{ IDUser = idLogged, IDFriend = idFriend};

            _logger.LogInformation("Calling delete friend command using mediator... ");
            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogInformation($"Message with the id = {idFriend} not found.");
                return NotFound();
            }

            return NoContent();
        }
    }
}
