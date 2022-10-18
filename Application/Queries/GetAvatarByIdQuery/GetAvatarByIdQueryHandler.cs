using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAvatarByIdQuery
{
    public class GetAvatarByIdQueryHandler : IRequestHandler<GetAvatarByIdQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvatarByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(GetAvatarByIdQuery info, CancellationToken cancellationToken)
        {
            var response = await _unitOfWork.UserRepository.GetAvatarByIdAsync(info.IdUser);

            if (response == null)
            {
                return null;
            }

            return response;
        }
    }
}
