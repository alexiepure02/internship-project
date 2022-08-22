using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendOfUserQuery
{
    public class GetFriendOfUserQueryHandler : IRequestHandler<GetFriendOfUserQuery, Friends>
    {
        private IAppDbContext _appDbContext;

        public GetFriendOfUserQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Friends> Handle(GetFriendOfUserQuery info, CancellationToken cancellationToken)
        {
            var friend = await _appDbContext.Friends.Where(u => u.IDUser == info.IDUser && u.IDFriend == info.IDFriend).FirstOrDefaultAsync();

            if (friend == null)
            {
                return null;
            }

            return friend;
        }
    }
}
