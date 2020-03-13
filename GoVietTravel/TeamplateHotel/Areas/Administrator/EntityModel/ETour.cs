using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProjectLibrary.Database;
using System;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ETour
    {
        public int ID { get; set; }


        [DisplayName("Menu")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Menu")]
        public int MenuID { get; set; }

        [DisplayName("Title")]
        [MaxLength(250)]
        [Required]
        public string Title { get; set; }
        [DisplayName("Address")]
      
        [Required]
        public string Address { get; set; }

        [DisplayName("Alias")]
        public string Alias { get; set; }

        [MaxLength]
        [Required]
        public string Image { get; set; }

        public string Description { get; set; }

        public int? Index { get; set; }

        [DisplayName("Meta Title")]
        [MaxLength(250)]
        public string MetaTitle { get; set; }

        [MaxLength(4000)]
        public string MetaDescription { get; set; }

        public bool Status { get; set; }

        public bool Hot { get; set; }

        public bool Sale { get; set; }

        public bool HotelService { get; set; }

        public decimal ThreeStar { get; set; }

      
        public decimal FourStar { get; set; }

        public decimal FiveStar { get; set; }

        public bool Home { get; set; }
        public bool HighlightTour { get; set; }
        public bool CheckCruise { get; set; }

        public bool ViewAll { get; set; }

        public decimal PriceSale { get; set; }

        public decimal Price { get; set; }

        public bool TourOther { get; set; }

        public string Content { get; set; }

        //[DisplayName("Điểm đi")]
        //[MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        //[Required(ErrorMessage = "Vui lòng nhập điểm đi")]
        public string ToDes { get; set; }

        //[DisplayName("Điểm đến")]
        //[MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        //[Required(ErrorMessage = "Vui lòng nhập điểm đến")]
        public string FromDes { get; set; }

        [DisplayName("Deal")]
        public bool Deal { get; set; }

        [DisplayName("Ngày bắt đầu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
               ApplyFormatInEditMode = true)]
        public DateTime? StartDeal { get; set; }

        [DisplayName("Đặt cọc (%)")]
        [Range(0, 100, ErrorMessage = "0-100%")]
        public int Deposit { get; set; }

        [DisplayName("Thời gian")]
        public int? TimeDeal { get; set; }

        public List<EGalleryITem> EGalleryITems { get; set; }

        public List<TabTour> TabTours { get; set; }
    }
}