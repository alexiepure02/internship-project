namespace WebPresentation.Dto
{
    public class MessageGetDto
    {
        public int ID { get; set; }
        public int IDSender { get; set; }
        public int IDReceiver { get; set; }
        public string Text { get; set; }
        public string DateTime { get; set; }
    }
}
