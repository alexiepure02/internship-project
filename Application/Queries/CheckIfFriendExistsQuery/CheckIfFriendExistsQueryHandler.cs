using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckIfFriendExistsQuery
{
    public class CheckIfFriendExistsQueryHandler : IRequestHandler<CheckIfFriendExistsQuery, bool>
    {
        private IAppDbContext _appDbContext;

        public CheckIfFriendExistsQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Handle(CheckIfFriendExistsQuery info, CancellationToken cancellationToken)
        {
            var friend = await _appDbContext.Friends.Where(u => u.IDUser == info.IDUser && u.IDFriend == info.IDFriend).FirstOrDefaultAsync();

            return friend != null;
        }
    }
}
