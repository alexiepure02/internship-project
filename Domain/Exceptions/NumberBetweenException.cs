using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatApp
{
    public class NumberBetweenException : Exception
    {
        public NumberBetweenException(int n) : base($"Error: Choose a number between 1 and {n}.")
        {
        }
    }
}
