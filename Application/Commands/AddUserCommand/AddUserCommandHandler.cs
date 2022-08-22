using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddUserCommand
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
    {
        private IAppDbContext _appDbContext;

        public AddUserCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(AddUserCommand info, CancellationToken cancellationToken)
        {
            _appDbContext.Users.Add(info.User);
            await _appDbContext.SaveChanges();

            return Unit.Value;
        }
    }
}
