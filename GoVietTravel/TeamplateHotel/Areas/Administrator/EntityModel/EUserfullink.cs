using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EUserfullink
    {
        public int ID { get; set; }

        [DisplayName("Tên ảnh")]
        public string Name { get; set; }

        [DisplayName("Hình ảnh")]
        [Required(ErrorMessage = "Vui lòng lựa chọn hình ảnh")]
        public string Image { get; set; }
        public string Url { get; set; }
        public int Location { get; set; }
        public int Type { get; set; }
        public int MenuCountry { get; set; }
    }
}