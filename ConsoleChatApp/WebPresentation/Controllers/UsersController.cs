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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsersController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserPutPostDto user)
        {
            var command = new CreateUserCommand
            {
                Username = user.Username,
                Password = user.Password,
                DisplayName = user.DisplayName
            };

            var result = await _mediator.Send(command);
            var mappedResult = _mapper.Map<UserGetDto>(result);

            return CreatedAtAction(nameof(GetById), new { id = mappedResult.ID }, mappedResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            var mappedResult = _mapper.Map<List<UserGetDto>>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUserByIdQuery { IDUser = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            var mappedResult = _mapper.Map<UserGetDto>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{username}/{password}")]
        public async Task<IActionResult> GetByAccount(string Username, string Password)
        {
            var query = new GetUserByAccountQuery { Username = Username, Password = Password };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            var mappedResult = _mapper.Map<UserGetDto>(result);
            return Ok(mappedResult);
        }
    }
}
