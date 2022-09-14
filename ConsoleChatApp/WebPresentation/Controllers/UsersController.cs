using Application.Commands.CreateUserCommand;
using Application.Queries.GetAllUsersQuery;
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
    }
}
