using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class FriendRequests
    {
        public int ID { get; set; }
        
        public int IDUser { get; set; }
        [ForeignKey(nameof(IDUser))]
        public User User { get; set; }
        
        public int IDRequester { get; set; }
        [ForeignKey(nameof(IDRequester))]
        public User Requester { get; set; }
    }
}
