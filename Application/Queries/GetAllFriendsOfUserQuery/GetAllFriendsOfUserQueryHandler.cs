using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendsOfUserQuery
{
    public class GetAllFriendsOfUserQueryHandler : IRequestHandler<GetAllFriendsOfUserQuery, List<Friends>>
    {
        private IAppDbContext _appDbContext;

        public GetAllFriendsOfUserQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Friends>> Handle(GetAllFriendsOfUserQuery info, CancellationToken cancellationToken)
        {
            var friends = await _appDbContext.Friends.Where(f => f.IDUser == info.IDUser).ToListAsync();

            if (friends == null)
            {
                return null;
            }
            return friends;
        }
    }
}
