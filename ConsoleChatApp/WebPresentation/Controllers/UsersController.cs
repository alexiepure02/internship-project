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
using Domain;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IActionResult> Login(string userName, string Password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null && await _userManager.CheckPasswordAsync(user, Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.DisplayName)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetConnectionString("SigningKey")));

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:7228/",
                    audience: "http://127.0.0.1:5173/",
                    claims: authClaims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserPutPostDto userInfo)
        {
            var userExists = await _userManager.FindByNameAsync(userInfo.UserName);

            if (userExists != null)
                return BadRequest("User already exists.");

            User user = new()
            {
                UserName = userInfo.UserName,
                DisplayName = userInfo.DisplayName
            };

            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to create user.");
            }

            return Ok("User created succesfully.");
        }

        [HttpPost]
        [Route("assign-role")]
        public async Task<IActionResult> AddToRole(string userName, string roleName)
        {
            var userExists = await _userManager.FindByNameAsync(userName);

            if (userExists == null)
            {
                return BadRequest("User does not exist.");
            }

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                var roleAdded = await _roleManager.CreateAsync(new IdentityRole<int>
                {
                    Name = roleName
                });
            }

            var adddRoleToUser = await _userManager.AddToRoleAsync(userExists, roleName);

            if (!adddRoleToUser.Succeeded)
            {
                return BadRequest("Failed to add user to role.");
            }

            return Ok($"User addded succesfully to {roleName} role.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserPutPostDto user)
        {
            _logger.LogInformation("Creating create user command using data from body... ");
            var command = new CreateUserCommand
            {
                Username = user.UserName,
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
            var mappedResult = _mapper.Map<List<UserGetDto>>(result);

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
            var mappedResult = _mapper.Map<List<UserGetDto>>(result);

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

        [HttpPut]
        [Route("friend-requests/{accepted}")]
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
            var result = await _mediator.Send(command);

            /*if (result.IsCompleted == false)
            {
                _logger.LogInformation("Command not completed.");
                return NotFound();
            }*/

            return NoContent();
        }
    }
}
