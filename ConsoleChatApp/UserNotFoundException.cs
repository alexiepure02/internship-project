using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatApp
{
    internal class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("Error: User not found.")
        {
        }
    }
}
