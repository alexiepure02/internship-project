using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateMessageCommand
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Message>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Message> Handle(CreateMessageCommand info, CancellationToken cancellationToken)
        {
            var message = new Message
            {
                IDSender = info.IDSender,
                IDReceiver = info.IDReceiver,
                Text = info.Text,
                DateTime = DateTime.UtcNow.ToString()
            };
            await _unitOfWork.MessageRepository.CreateMessageAsync(message);
            await _unitOfWork.Save();

            return message;
        }
    }
}
