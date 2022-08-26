using System.ComponentModel.DataAnnotations;

namespace WebPresentation.Dto
{
    public class UserPutPostDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; }
    }
}
