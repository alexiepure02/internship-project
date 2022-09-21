using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidMessageException : Exception
    {
        public InvalidMessageException() : base("Invalid message.")
        {
        }
    }
}
