using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatApp.Domain
{
    public class Message
    {
        public int IdSender { get; set; }
        public int IdReceiver { get; set; }
        public string Text { get; set; }
        public string DateTime { get; set; }

    }
}
