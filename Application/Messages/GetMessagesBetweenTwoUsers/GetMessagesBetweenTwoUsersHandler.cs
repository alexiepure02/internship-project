using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.GetMessagesBetweenTwoUsers
{
    internal class GetMessagesBetweenTwoUsersHandler : IRequestHandler<GetMessagesBetweenTwoUsers, List<Message>>
    {
        private IMessageRepository _messageRepository;

        public GetMessagesBetweenTwoUsersHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<List<Message>> Handle(GetMessagesBetweenTwoUsers ids, CancellationToken cancellationToken)
        {
            var newMessages = _messageRepository.GetMessagesBetweenTwoUsers(ids.IdSender, ids.IdReceiver);

            return Task.FromResult(newMessages);
        }
    }
}
