﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserInFriendsException : Exception
    {
        public UserInFriendsException(int id) : base($"Error: User with the id {id} is already a friend.")
        {
        }
    }
}
