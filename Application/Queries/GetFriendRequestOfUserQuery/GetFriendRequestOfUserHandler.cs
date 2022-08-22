using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendRequestOfUserQuery
{
    public class GetFriendRequestOfUserQueryHandler : IRequestHandler<GetFriendRequestOfUserQuery, FriendRequests>
    {
        private IAppDbContext _appDbContext;

        public GetFriendRequestOfUserQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<FriendRequests> Handle(GetFriendRequestOfUserQuery info, CancellationToken cancellationToken)
        {
            var friendRequest = await _appDbContext.FriendRequests.Where(u => u.IDUser == info.IDUser && u.IDRequester == info.IDRequester).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return null;
            }

            return friendRequest;
        }
    }
}
