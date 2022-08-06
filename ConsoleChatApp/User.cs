﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatApp
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public int Id { get; set; }
        public List<int> Friends { get; set; }
        public List<int> FriendRequests { get; set; }
    }
}
