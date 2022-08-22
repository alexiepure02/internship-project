using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckIfFriendRequestExistsQuery
{
    public class CheckIfFriendRequestExistsQueryHandler : IRequestHandler<CheckIfFriendRequestExistsQuery, bool>
    {
        private IAppDbContext _appDbContext;

        public CheckIfFriendRequestExistsQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(CheckIfFriendRequestExistsQuery info, CancellationToken cancellationToken)
        {
            var friendRequest = await _appDbContext.FriendRequests.Where(u => u.IDUser == info.IDUser && u.IDRequester == info.IDRequester).FirstOrDefaultAsync();

            return friendRequest != null;
        }
    }
}
