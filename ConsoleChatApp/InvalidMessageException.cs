﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatApp
{
    internal class InvalidMessageException : Exception
    {
        public InvalidMessageException(string? message) : base(message)
        {
        }
    }
}
