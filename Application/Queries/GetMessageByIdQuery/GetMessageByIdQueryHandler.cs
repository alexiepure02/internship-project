using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetMessageByIdQuery
{
    public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, Message>
    {
        private IUnitOfWork _unitOfWork;

        public GetMessageByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Message> Handle(GetMessageByIdQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.MessageRepository.GetMessageByIdAsync(info.IDMessage);
        }
    }
}
