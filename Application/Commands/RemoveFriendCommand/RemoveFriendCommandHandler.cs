using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.RemoveFriendCommand
{
    public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand>
    {
        private IAppDbContext _appDbContext;

        public RemoveFriendCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(RemoveFriendCommand info, CancellationToken cancellationToken)
        {
            // user 1 and user 2 are friends
            // in database:
            // - IDUser = 1 and IDFriend = 2
            // - IDUser = 2 and IDFriend = 1

            // so you have to remove 2 instances

            var friend1 = await _appDbContext.Friends
                .Where(f => f.IDUser == info.IDUser && f.IDFriend == info.IDFriend)
                .FirstOrDefaultAsync();

            var friend2 = await _appDbContext.Friends
                .Where(f => f.IDUser == info.IDFriend && f.IDFriend == info.IDUser)
                .FirstOrDefaultAsync();

            if (friend1 == null || friend2 == null)
            {
                return default;
            }

            _appDbContext.Friends.Remove(friend1);
            _appDbContext.Friends.Remove(friend2);
            await _appDbContext.SaveChanges();

            return Unit.Value;
        }
    }
}
