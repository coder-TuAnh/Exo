using System.ComponentModel;

namespace TeamplateHotel.Areas.Administrator.ModelShow
{
    public class ShowArticle
    {
        public int ID { get; set; }

        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [DisplayName("Chuyên mục")]
        public string TitleMenu { get; set; }

        [DisplayName("Thứ tự hiển thị")]
        public int? Index { get; set; }

        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }

        [DisplayName("Bài viết giới thiệu")]
        public bool Home { get; set; }

        [DisplayName("Bài viết hot")]
        public bool Hot { get; set; }

        [DisplayName("Nhân viên")]
        public bool About { get; set; }

        [DisplayName("Bài viết new")]
        public bool New { get; set; }

        [DisplayName("Bài viết việt nam")]
        public bool TripvietNam { get; set; }
        
        [DisplayName("Bài viết Lào")]
        public bool TripLao { get; set; }

        [DisplayName("Bài viết cambodia")]
        public bool TripCambodia { get; set; }
    }
}