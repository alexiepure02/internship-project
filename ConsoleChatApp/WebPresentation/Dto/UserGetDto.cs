namespace WebPresentation.Dto
{
    public class UserGetDto
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }

        // not sure if i should include these in here

        // public ICollection<Message> Messages { get; set; }

        //public virtual IList<Friends> Friends { get; set; }

        // public virtual IList<FriendRequests> FriendRequests { get; set; }
    }
}
