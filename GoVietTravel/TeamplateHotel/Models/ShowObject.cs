using System;

namespace TeamplateHotel.Models
{
    public class ShowObject
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string MenuAlias { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? Index { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public string SecondMenu { get; set; }
        public int? TimeDeal { get; set; }
        public DateTime? StartDeal { get; set; }
        public int MenuID { get; internal set; }
        public decimal timesdown { get; set; }
        public string Address { get; set; }
    }
}