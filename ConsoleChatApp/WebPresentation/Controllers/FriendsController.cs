using Application.Commands.DeleteFriendCommand;
using Application.Queries.GetAllFriendsOfUserQuery;
using Application.Queries.GetFriendOfUserQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Dto;

namespace WebPresentation.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        // add DataAnnotations

        public FriendsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAllFriendsOfUser(int id)
        {
            var query = new GetAllFriendsOfUserQuery { IDUser = id};
            var result = await _mediator.Send(query);
            var mappedResult = _mapper.Map<List<FriendGetDto>>(result);
            return Ok(mappedResult);
        }

        [HttpGet]
        [Route("{idLogged}/friends/{idFriend}")]
        public async Task<IActionResult> GetFriendOfUser(int idLogged, int idFriend)
        {
            var query = new GetFriendOfUserQuery { IDUser = idLogged, IDFriend = idFriend };
            var result = await _mediator.Send(query);
            var mappedResult = _mapper.Map<FriendGetDto>(result);
            return Ok(mappedResult);
        }

        [HttpDelete]
        [Route("{idLogged}/friends/{idFriend}")]
        public async Task<IActionResult> DeleteFriend(int idLogged, int idFriend)
        {
            var command = new DeleteFriendCommand{ IDUser = idLogged, IDFriend = idFriend};
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound();

            return NoContent();
        }
    }
}
