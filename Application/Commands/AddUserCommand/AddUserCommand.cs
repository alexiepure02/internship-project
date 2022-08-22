﻿using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddUserCommand
{
    public class AddUserCommand : IRequest
    {
        public User User { get; set; }
    }
}
