using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByIdQuery
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private IAppDbContext _appDbContext;

        public GetUserByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> Handle(GetUserByIdQuery info, CancellationToken cancellationToken)
        {
            var user = await _appDbContext.Users.Where(u => u.ID == info.Id).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
