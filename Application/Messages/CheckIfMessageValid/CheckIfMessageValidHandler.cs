using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.CheckIfMessageValid
{
    public class CheckIfMessageValidHandler : IRequestHandler<CheckIfMessageValid>
    {
        private IMessageRepository _messageRepository;

        public CheckIfMessageValidHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<Unit> Handle(CheckIfMessageValid message, CancellationToken cancellationToken)
        {
            _messageRepository.CheckIfMessageValid(message.Message);

            return Task.FromResult(Unit.Value);
        }
    }
}
