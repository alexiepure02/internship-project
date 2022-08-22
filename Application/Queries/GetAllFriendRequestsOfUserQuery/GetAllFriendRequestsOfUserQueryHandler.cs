using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendRequestsOfUserQuery
{
    internal class GetAllFriendRequestsOfUserQueryHandler : IRequestHandler<GetAllFriendRequestsOfUserQuery, List<FriendRequests>>
    {
        private IAppDbContext _appDbContext;

        public GetAllFriendRequestsOfUserQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<FriendRequests>> Handle(GetAllFriendRequestsOfUserQuery info, CancellationToken cancellationToken)
        {
            var friendRequests =  await _appDbContext.FriendRequests.Where(f => f.IDUser == info.IDUser).ToListAsync();

            if (friendRequests == null)
            {
                return null;
            }
            return friendRequests;
        }
    }
}
