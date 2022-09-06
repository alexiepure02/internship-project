using System.ComponentModel.DataAnnotations;

namespace WebPresentation.Dto
{
    public class MessagePutPostDto
    {
        [Required]
        public int IDSender { get; set; }
        [Required]
        public int IDReceiver { get; set; }
        [Required]
        public string Text { get; set; }

    }
}
