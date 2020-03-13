using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProjectLibrary.Database;
using System;


namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ECruise
    {

        public int ID { get; set; }

        [DisplayName("Tên chuyến đi")]
        [Required(ErrorMessage = "Vui lòng nhập tên chuyến đi")]
        public string Name { get; set; }

        public string Alias { get; set; }

        public double Rate { get; set; }

        public DateTime? UpdateDay { get; set; }

        [DisplayName("Địa điểm")]
        [Required(ErrorMessage = "Vui lòng nhập địa điểm")]
        public string Location { get; set; }

        public double Star { get; set; }

        public string Action { get; set; }

        [DisplayName("Giá niêm yết")]
        [Required(ErrorMessage = "Giá niêm yết")]
        public double Price { get; set; }

        [DisplayName("Giá khuyến mãi")]
        [Required(ErrorMessage = "Giá khuyến mãi")]
        public double PriceSale { get; set; }

        public string Duration { get; set; }

        public string Departure { get; set; }

        public string[] MenuID { get; set; }

        public string Image { get; set; }

        public string Cruisegallery { get; set; }

        public string Description { get; set; }

        public bool BestCruise { get; set; }

        public string About { get; set; }

        [DisplayName("Thẻ mô tả")]
        [Required(ErrorMessage = "Thẻ mô tả")]
        public string MetaDescription { get; set; }
        [DisplayName("Thẻ mô từ khóa")]
        [Required(ErrorMessage = "Thẻ mô từ khóa")]
        public string MetaKeywords { get; set; }

        public string Freeservic { get; set; }

        public bool Home { get; set; }

        public List<Cruisetab> Cruitabs { get; set; }
    }
    public enum Duration
    {
        [Display(Name = "Full Day")]
        FullDay = 1,
        [Display(Name = "2 Days/1 Night")]
        Day2N1 = 1,
        [Display(Name = "3 Days/2 Night")]
        Day3N2 = 2,
    }
    public class CruiseGallery
    {
        public string NameImages { get; set; }
    }
    public class ESerivceCruise
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public double? Price { get; set; }
        public string Unit { get; set; }
        public string Content { get; set; }
        public string CatalogServic { get; set; }
    }
    public enum CatalogServic
    {
        [Display(Name = "Services")]
        Services = 1,
        [Display(Name = "Offers")]
        Offers = 2,

    }
}