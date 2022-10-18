using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Friends
    {
        public int ID { get; set; }
        
        public int IDUser { get; set; }
        [ForeignKey(nameof(IDUser))]
        public User User { get; set; }

        public int IDFriend { get; set; }
        [ForeignKey(nameof(IDFriend))]
        public User Friend { get; set; }
    }
}
