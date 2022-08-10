using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.AddMessages
{
    public class AddMessagesHandler : IRequestHandler<AddMessages>
    {
        private IMessageRepository _messageRepository;
        
        public AddMessagesHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<Unit> Handle(AddMessages messages, CancellationToken cancellationToken)
        {
            _messageRepository.AddMessages(messages.Messages);

            return Task.FromResult(Unit.Value);
        }
    }
}
