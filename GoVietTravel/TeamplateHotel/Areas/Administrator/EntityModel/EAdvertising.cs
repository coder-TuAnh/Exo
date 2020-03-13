using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EAdvertising
    {
        public int ID { get; set; }

        [DisplayName("Title")]
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [DisplayName("Url")]
        [Url]
        [MaxLength]
        public string Url { get; set; }

        [DisplayName("Image")]
        [Required]
        public string Image { get; set; }

        [DisplayName("New tab")]
        public string Target { get; set; }

        [DefaultValue(0)]
        public int? Index { get; set; }

        public bool Status { get; set; }
    }
}