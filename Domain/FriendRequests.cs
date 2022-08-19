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

        [ForeignKey(nameof(IDRequester))]
        public int IDRequester { get; set; }
        //public User Requester { get; set; }

        [ForeignKey(nameof(IDRequester))]
        public int IDRequested { get; set; }
        //public User Requested { get; set; }
    }
}
