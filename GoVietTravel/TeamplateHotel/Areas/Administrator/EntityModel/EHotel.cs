using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EHotel
    {
        public int ID { get; set; }

        public string LanguageID { get; set; }

        [Required]
        [MaxLength]
        public string Name { get; set; }

        [Required]
        public string Logo { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [MaxLength]
        public string Tel { get; set; }

        [MaxLength]
        public string Fax { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

        //[Required]
        [MaxLength(20)]
        public string CodeBooking { get; set; }

        [Required]
        public string Website { get; set; }


        public string Tripadvisor { get; set; }


        public string FaceBook { get; set; }


        public string Instagram { get; set; }


        public string Twitter { get; set; }


        public string Youtube { get; set; }

        [Required]
        public string CopyRight { get; set; }

        [MaxLength(1000)]
        public string MetaTitle { get; set; }

        [MaxLength(4000)]
        public string MetaDescription { get; set; }

        [DisplayName("Term & Condition")]
        //[Required]
        public string Condition { get; set; }


        [Required]
        public string Hotline { get; set; }

    }
}