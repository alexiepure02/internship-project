using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatApp.Domain.Exceptions
{
    public class SameIdException : Exception
    {
        public SameIdException(int id) : base($"Error: {id} is your ID.")
        {
        }
    }
}
