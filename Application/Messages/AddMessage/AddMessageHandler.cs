using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.AddMessage
{
    public class AddMessageHandler : IRequestHandler<AddMessage>
    {
        private IMessageRepository _messageRepository;

        public AddMessageHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<Unit> Handle(AddMessage info, CancellationToken cancellationToken)
        {
            _messageRepository.AddMessage(info.Sender, info.Receiver, info.Message);

            return Task.FromResult(Unit.Value);
        }
    }
}
