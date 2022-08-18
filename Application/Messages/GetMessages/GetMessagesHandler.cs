using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.GetMessages
{
    public class GetMessagesHandler : IRequestHandler<GetMessages, List<Message>>
    {
        private IMessageRepository _messageRepository;

        public GetMessagesHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<List<Message>> Handle(GetMessages request, CancellationToken cancellationToken)
        {
            List<Message> messages = _messageRepository.GetMessages();

            return Task.FromResult(messages);
        }
    }
}
