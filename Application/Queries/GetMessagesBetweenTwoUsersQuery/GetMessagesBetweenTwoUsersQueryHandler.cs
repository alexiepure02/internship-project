using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetMessagesBetweenTwoUsersQuery
{
    internal class GetMessagesBetweenTwoUsersQueryHandler : IRequestHandler<GetMessagesBetweenTwoUsersQuery, List<Message>>
    {
        private IUnitOfWork _unitOfWork;

        public GetMessagesBetweenTwoUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Message>> Handle(GetMessagesBetweenTwoUsersQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.MessageRepository.GetMessagesBetweenTwoUsersAsync(info.IDUser1, info.IDUser2);
        }
    }
}
