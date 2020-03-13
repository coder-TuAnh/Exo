using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EUser
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Username")]
        [MinLength(8)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public bool Status { get; set; }
        public bool UserContent { get; set; }
        public string PasswordOld { get; set; }
    }
}