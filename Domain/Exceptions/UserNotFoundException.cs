﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string user) : base($"Error: User {user} not found.")
        {
        }
        public UserNotFoundException(int id) : base($"Error: User with the id {id} not found.")
        {
        }
    }
}
