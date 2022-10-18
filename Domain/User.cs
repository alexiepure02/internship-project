using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Domain
{
    public class User : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public string AvatarUri { get; set; }
        public ICollection<Message> Messages { get; set; }
        public virtual ICollection<Friends> MainUserFriends { get; set; }
        public virtual IList<Friends> Friends { get; set; }
        public virtual IList<FriendRequests> FriendRequests { get; set; }
    }
}
