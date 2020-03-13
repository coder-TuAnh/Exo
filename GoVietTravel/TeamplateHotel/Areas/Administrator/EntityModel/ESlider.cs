using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ESlider
    {
        public int ID { get; set; }

        public int LanguageID { get; set; }

        public string MenuIDs { get; set; }
        public int? MenuID { get; set; }

        [MaxLength]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Image { get; set; }

        [Url]
        public string Link { get; set; }

        public int? Index { get; set; }

        public bool ViewAll { get; set; }

        public bool Status { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }
    }
}