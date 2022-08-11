﻿using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.RemoveUser
{
    public class RemoveUser : IRequest
    {
        public User User { get; set; }
    }
}