using System.ComponentModel.DataAnnotations;

namespace WebPresentation.Dto
{
    public class FriendRequestPutPostDto
    {
        [Required]
        public int IDUser { get; set; }
        [Required]
        public int IDRequester { get; set; }
    }
}
