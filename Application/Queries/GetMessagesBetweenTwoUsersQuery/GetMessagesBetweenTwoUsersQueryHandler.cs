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
        private IAppDbContext _appDbContext;

        public GetMessagesBetweenTwoUsersQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Message>> Handle(GetMessagesBetweenTwoUsersQuery info, CancellationToken cancellationToken)
        {
            var messages = await _appDbContext.Messages
                .Where(m => m.IDSender == info.idUser1 && m.IDReceiver == info.idUser2 ||
                m.IDSender == info.idUser2 && m.IDReceiver == info.idUser1)
                .ToListAsync();
        
            if (messages == null)
            {
                return null;
            }
            return messages.OrderBy(m => m.DateTime).ToList();
        }
    }
}
