using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByAccountQuery
{
    public class GetUserByAccountQueryHandler : IRequestHandler<GetUserByAccountQuery, User>
    {
        private IAppDbContext _appDbContext;

        public GetUserByAccountQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> Handle(GetUserByAccountQuery info, CancellationToken cancellationToken)
        {
            var user = await _appDbContext.Users.Where(u => u.Username == info.Username && u.Password == info.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
