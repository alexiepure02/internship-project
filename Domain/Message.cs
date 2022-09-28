using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Message
    {
        public int ID { get; set; }

        public int IDSender { get; set; }
        [ForeignKey(nameof(IDSender))]
        public User Sender { get; set; }
        
        public int IDReceiver { get; set; }
        [ForeignKey(nameof(IDReceiver))]
        public User Receiver { get; set; }
        public string Text { get; set; }
        public string DateTime { get; set; }

    }
}
