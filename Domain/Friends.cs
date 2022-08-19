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
        [ForeignKey(nameof(IDUser))]
        public int IDUser { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(IDFriend))]
        public int IDFriend { get; set; }
        public User Friend { get; set; }
    }
}
