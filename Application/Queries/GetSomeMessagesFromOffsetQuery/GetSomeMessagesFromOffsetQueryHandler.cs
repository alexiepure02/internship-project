using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetSomeMessagesFromOffsetQuery
{
    public class GetSomeMessagesFromOffsetQueryHandler : IRequestHandler<GetSomeMessagesFromOffsetQuery, List<Message>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSomeMessagesFromOffsetQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Message>> Handle(GetSomeMessagesFromOffsetQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.MessageRepository.GetSomeMessagesFromOffset(info.IDUser1, info.IDUser2, info.Offset, info.NumberOfMessages);
        }
    }
}
