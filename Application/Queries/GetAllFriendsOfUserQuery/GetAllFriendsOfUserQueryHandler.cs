using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendsOfUserQuery
{
    public class GetAllFriendsOfUserQueryHandler : IRequestHandler<GetAllFriendsOfUserQuery, List<Friends>>
    {
        private IUnitOfWork _unitOfWork;

        public GetAllFriendsOfUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Friends>> Handle(GetAllFriendsOfUserQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetAllFriendsOfUserAsync(info.IDUser);
        }
    }
}
