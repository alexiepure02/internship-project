using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.ValidateIdFriend
{
    public class ValidateIdFriend : IRequest
    {
        public int IdUser { get; set; }
        public int IdFriend { get; set; }
    }
}
