namespace Domain
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }

        //public List<int> Friends { get; set; }
        //public List<int> FriendRequests { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<int> Friends { get; set; }
        public ICollection<int> FriendRequests { get; set; }
    }
}
