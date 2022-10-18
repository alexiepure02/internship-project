using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetNumberOfMessagesBetweenTwoUsersQuery
{
    internal class GetNumberOfMessagesBetweenTwoUsersQueryHandler : IRequestHandler<GetNumberOfMessagesBetweenTwoUsersQuery, int>
    {
        private IUnitOfWork _unitOfWork;

        public GetNumberOfMessagesBetweenTwoUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetNumberOfMessagesBetweenTwoUsersQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.MessageRepository.GetNumberOfMessagesBetweenTwoUsersAsync(info.IDUser1, info.IDUser2);
        }
    }
}
