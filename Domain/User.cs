﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public ICollection<Message> Messages { get; set; }
        public virtual ICollection<Friends> MainUserFriends { get; set; }
        public virtual IList<Friends> Friends { get; set; }
        public virtual IList<FriendRequests> FriendRequests { get; set; }
    }
}
