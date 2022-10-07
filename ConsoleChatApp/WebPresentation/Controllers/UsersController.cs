using Application.Commands.AddToRoleCommand;
using Application.Commands.CreateFriendRequestCommand;
using Application.Commands.DeleteFriendCommand;
using Application.Commands.LoginCommand;
using Application.Commands.RegisterCommand;
using Application.Commands.UpdateFriendRequestCommand;
using Application.Commands.UpdateUserCommand;
using Application.Queries.GetAllFriendRequestsOfUserQuery;
using Application.Queries.GetAllFriendsOfUserQuery;
using Application.Queries.GetAllUsersQuery;
using Application.Queries.GetFriendOfUserQuery;
using Application.Queries.GetFriendRequestByIdQuery;
using Application.Queries.GetFriendRequestOfUserQuery;
using Application.Queries.GetUserByIdQuery;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebPresentation.Dto;

namespace WebPresentation.Controllers
{
    [EnableCors("ClientPermission")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        private readonly IConfiguration _configuration;

        public UsersController(IMapper mapper, IMediator mediator, ILogger<UsersController> logger, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginDto userInfo)
        {

             _logger.LogInformation("Creating login command... ");
            var query = new LoginCommand
            {
                UserName = userInfo.UserName,
                Password = userInfo.Password,
            };

            _logger.LogInformation("Calling login command using mediator... ");
            var result = await _mediator.Send(query);
             
            if (result == null)
            {
                _logger.LogInformation($"User with the username={userInfo.UserName} and " +
                    $"password={userInfo.Password} not found.");
                return Unauthorized();
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(result),
                expiration = result.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserPutPostDto userInfo)
        {

            _logger.LogInformation("Creating register command... ");
            var query = new RegisterCommand
            {
                UserName = userInfo.UserName,
                Password = userInfo.Password,
                DisplayName = userInfo.DisplayName,
            };

            _logger.LogInformation("Calling register command using mediator... ");
            var result = await _mediator.Send(query);

            if (result == "User created succesfully.")
            {
                return Ok(result);
            }
            else
            {
                _logger.LogInformation(result);
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("assign-role")]
        public async Task<IActionResult> AddToRole(string userName, string roleName)
        {

            _logger.LogInformation("Creating add to role command... ");
            var query = new AddToRoleCommand
            {
                UserName = userName,
                RoleName = roleName,
            };

            _logger.LogInformation("Calling add to role command using mediator... ");
            var result = await _mediator.Send(query);

            if (result == $"User added succesfully to {roleName} role.")
            {
                return Ok(result);
            }
            else
            {
                _logger.LogInformation(result);
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
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
        [Route("{id}/friends")]
        [Authorize]
        public async Task<IActionResult> GetAllFriendsOfUser(int id)
        {
            _logger.LogInformation("Creating get all friends of user query... ");
            var query = new GetAllFriendsOfUserQuery { IDUser = id };

            _logger.LogInformation("Calling get all friends of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<UserGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}/friends/{idFriend}")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetAllFriendRequestsOfUser(int id)
        {
            _logger.LogInformation("Creating get all friend requests of user query... ");
            var query = new GetAllFriendRequestsOfUserQuery { IDUser = id };

            _logger.LogInformation("Calling get all friend requests of user query using mediator... ");
            var result = await _mediator.Send(query);

            _logger.LogInformation("Mapping result object to Dto object... ");
            var mappedResult = _mapper.Map<List<UserGetDto>>(result);

            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("friend-requests/{id}")]
        [Authorize]
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
        [Authorize]
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

        [HttpPut]
        [Route("friend-requests/{accepted}")]
        [Authorize]
        public async Task<IActionResult> UpdateFriendRequestAsync([FromBody] FriendRequestPutPostDto friendRequest, bool accepted)
        {
            _logger.LogInformation("Creating update friend request command... ");
            var command = new UpdateFriendRequestCommand
            {
                IDUser = friendRequest.IDUser,
                IDRequester = friendRequest.IDRequester,
                Accepted = accepted,

            };

            _logger.LogInformation("Calling update friend request command using mediator... ");
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}/{displayName}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, string displayName)
        {
            _logger.LogInformation("Creating update display name command... ");
            var command = new UpdateUserDisplayNameCommand
            {
                IdUser = id,
                NewDisplayName = displayName,
            };

            _logger.LogInformation("Calling update display name command using mediator... ");
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
