namespace WebPresentation.Dto
{
    public class UserGetDto
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public virtual IList<Friends> Friends { get; set; }

    }
}
