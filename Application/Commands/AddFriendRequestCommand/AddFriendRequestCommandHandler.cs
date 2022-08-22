using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddFriendRequestCommand
{
    public class AddFriendRequestCommandHandler : IRequestHandler<AddFriendRequestCommand>
    {
        private IAppDbContext _appDbContext;

        public AddFriendRequestCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(AddFriendRequestCommand info, CancellationToken cancellationToken)
        {
            _appDbContext.FriendRequests.Add(new FriendRequests
            {
                IDUser = info.IDUser,
                IDRequester = info.IDRequester
            });

            await _appDbContext.SaveChanges();

            return Unit.Value;
        }
    }
}
