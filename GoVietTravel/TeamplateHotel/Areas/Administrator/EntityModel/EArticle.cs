using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EArticle
    {
        public int ID { get; set; }

        [DisplayName("Menu")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Pelese select menu")]
        public int MenuID { get; set; }


        [Required]
        [MaxLength]
        public string Title { get; set; }

        [MaxLength]
        public string Alias { get; set; }

        [MaxLength]
        public string Image { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Contet")]
        public string Content { get; set; }

        public int? Index { get; set; }

        [DisplayName("Meta title")]
        [MaxLength]
        public string MetaTitle { get; set; }

        [DisplayName("Description")]
        [MaxLength]
        public string MetaDescription { get; set; }

        public bool Status { get; set; }

        [DisplayName("Show home")]
        public bool Home { get; set; }

        public bool Hot { get; set; }

        public bool New { get; set; }

        public bool TripCambodia { get; set; }

        public bool TripLao { get; set; }

        public bool TripVietNam { get; set; }

    }
}