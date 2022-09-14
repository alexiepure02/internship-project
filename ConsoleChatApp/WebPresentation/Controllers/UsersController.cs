using Application.Commands.CreateFriendRequestCommand;
using Application.Commands.CreateUserCommand;
using Application.Commands.DeleteFriendCommand;
using Application.Commands.UpdateFriendRequestCommand;
using Application.Queries.GetAllFriendRequestsOfUserQuery;
using Application.Queries.GetAllFriendsOfUserQuery;
using Application.Queries.GetAllUsersQuery;
using Application.Queries.GetFriendOfUserQuery;
using Application.Queries.GetFriendRequestByIdQuery;
using Application.Queries.GetFriendRequestOfUserQuery;
using Application.Queries.GetUserByAccountQuery;
using Application.Queries.GetUserByIdQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Dto;

namespace WebPresentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper, IMediator mediator, ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserPutPostDto user)
        {
            _logger.LogInformation("Creating create user command using data from body... ");
            var command = new CreateUserCommand
            {
                Username = user.Username,
                Password = user.Password,
                DisplayName = user.DisplayName
            };

            _logger.LogInformation("Calling create user command using mediator... ");
            var result = await _mediator.Send(command);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<UserGetDto>(result);

            return CreatedAtAction(nameof(GetById), new { id = mappedResult.ID }, mappedResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Calling get users query... ");
            var result = await _mediator.Send(new GetAllUsersQuery());

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<UserGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Creating get user by id query... ");
            var query = new GetUserByIdQuery { IDUser = id };

            _logger.LogInformation("Calling get user by id query using mediator... ");
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogInformation($"User with the id = {id} not found.");
                return NotFound();
            }

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<UserGetDto>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{username},{password}")]
        public async Task<IActionResult> GetByAccount(string username, string password)
        {
            _logger.LogInformation("Creating get user by account query... ");
            var query = new GetUserByAccountQuery { Username = username, Password = password };

            _logger.LogInformation("Calling get user by account query using mediator... ");
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogInformation($"User with the username = {username} and password = {password} not found.");
                return NotFound();
            }

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<UserGetDto>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}/friends")]
        public async Task<IActionResult> GetAllFriendsOfUser(int id)
        {
            _logger.LogInformation("Creating get all friends of user query... ");
            var query = new GetAllFriendsOfUserQuery { IDUser = id };

            _logger.LogInformation("Calling get all friends of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<FriendGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}/friends/{idFriend}")]
        public async Task<IActionResult> GetFriendOfUser(int id, int idFriend)
        {
            _logger.LogInformation("Creating get friend of user query... ");
            var query = new GetFriendOfUserQuery { IDUser = id, IDFriend = idFriend };

            _logger.LogInformation("Calling get friend of user query using mediator... ");
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogInformation($"Friend between user with the id = {idFriend} and user with the id = {id} not found.");
                return NotFound();
            }

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<FriendGetDto>(result);
            return Ok(mappedResult);
        }

        [HttpDelete]
        [Route("{id}/friends/{idFriend}")]
        public async Task<IActionResult> DeleteFriendOfUser(int id, int idFriend)
        {
            _logger.LogInformation("Creating delete friend command... ");
            var command = new DeleteFriendCommand { IDUser = id, IDFriend = idFriend };

            _logger.LogInformation("Calling delete friend command using mediator... ");
            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogInformation($"Friend between user with the id = {idFriend} and user with the id = {id} not found.");
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        [Route("friend-requests")]
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
        [Route("{id}/friend-requests")]
        public async Task<IActionResult> GetAllFriendRequestsOfUser(int id)
        {
            _logger.LogInformation("Creating get all friend requests of user query... ");
            var query = new GetAllFriendRequestsOfUserQuery { IDUser = id };

            _logger.LogInformation("Calling get all friend requests of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<FriendRequestGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("friend-requests/{id}")]
        public async Task<IActionResult> GetFriendRequestById(int id)
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
        [Route("{id}/friend-requests/{idRequester}")]
        public async Task<IActionResult> GetFriendRequestOfUser(int id, int idRequester)
        {
            _logger.LogInformation("Creating get friend request of user query... ");
            var query = new GetFriendRequestOfUserQuery { IDUser = id, IDRequester = idRequester };

            _logger.LogInformation("Calling get friend request of user query using mediator... ");
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogInformation($"Friend request between user with the id = {idRequester} and user with the id = {id} not found.");
                return NotFound();
            }

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<FriendRequestGetDto>(result);

            return Ok(mappedResult);
        }
         
        [HttpGet]
        [Route("{id}/friend-requests/{idRequester},{accepted}")]
        public IActionResult UpdateFriendRequest(int id, int idRequester, bool accepted)
        {
            _logger.LogInformation("Creating update friend request command... ");
            var command = new UpdateFriendRequestCommand
            {
                IDUser = id,
                IDRequester = idRequester,
                Accepted = accepted,

            };

            _logger.LogInformation("Calling update friend request command using mediator... ");
            var result = _mediator.Send(command);

            if (result.IsCompleted == false)
            {
                _logger.LogInformation("Command not completed.");
                return NotFound();
            }

            return NoContent();
        }
    }
}
