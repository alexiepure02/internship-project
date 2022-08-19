﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Message
    {
        [ForeignKey(nameof(IDSender))]
        public int IDSender { get; set; }
        public User Sender { get; set; }
        
        [ForeignKey(nameof(IDReceiver))]
        public int IDReceiver { get; set; }
        public User Receiver { get; set; }
        public string Text { get; set; }
        public string DateTime { get; set; }

    }
}
