﻿using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendRequestOfUserQuery
{
    public class GetFriendRequestOfUserQuery : IRequest<FriendRequests>
    {
        public int IDUser { get; set; }
        public int IDRequester { get; set; }
    }
}
