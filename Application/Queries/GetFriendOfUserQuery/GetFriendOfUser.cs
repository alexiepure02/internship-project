﻿using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendOfUserQuery
{
    public class GetFriendOfUserQuery : IRequest<Friends>
    {
        public int IDUser { get; set; }
        public int IDFriend { get; set; }
    }
}
