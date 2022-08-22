using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddMessageCommand
{
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand>
    {
        private IAppDbContext _appDbContext;

        public AddMessageCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(AddMessageCommand info, CancellationToken cancellationToken)
        {
            _appDbContext.Messages.Add(new Message
            {
                IDSender = info.IDSender,
                IDReceiver = info.IDReceiver,
                Text = info.Text,
                DateTime = DateTime.UtcNow.ToString()
            });
            await _appDbContext.SaveChanges();

            return Unit.Value;
        }
    }
}
