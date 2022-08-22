using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateFriendRequestCommand
{
    public class UpdateFriendRequestCommandHandler : IRequestHandler<UpdateFriendRequestCommand>
    {
        private IAppDbContext _appDbContext;

        public UpdateFriendRequestCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(UpdateFriendRequestCommand info, CancellationToken cancellationToken)
        {
            var friendRequest = await _appDbContext.FriendRequests.Where(f => f.IDUser == info.IDUser && f.IDRequester == info.IDRequester).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return default;
            }

            _appDbContext.Friends.Add(new Friends
            {
                IDUser = info.IDUser,
                IDFriend = info.IDRequester
            });

            _appDbContext.Friends.Add(new Friends
            {
                IDUser = info.IDRequester,
                IDFriend = info.IDUser
            });


            _appDbContext.FriendRequests.Remove(friendRequest);
            await _appDbContext.SaveChanges();

            return Unit.Value;
        }
    }
}
