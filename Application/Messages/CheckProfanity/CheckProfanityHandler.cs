using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.CheckProfanity
{
    public class CheckProfanityHandler : IRequestHandler<CheckProfanity, bool>
    {
        private IMessageRepository _messageRepository;

        public CheckProfanityHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<bool> Handle(CheckProfanity message, CancellationToken cancellationToken)
        {
            var isProfane = _messageRepository.CheckProfanity(message.Message);

            return Task.FromResult(isProfane);
        }
    }
}
