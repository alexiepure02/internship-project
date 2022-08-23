using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByAccountQuery
{
    public class GetUserByAccountQueryHandler : IRequestHandler<GetUserByAccountQuery, User>
    {
        private IUnitOfWork _unitOfWork;

        public GetUserByAccountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserByAccountQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetUserByAccountAsync(info.Username, info.Password);
        }
    }
}
