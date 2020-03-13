using System;

namespace TeamplateHotel.Models
{
    public class MBookTour
    {
        public int TourId { get; set; }
        public int TabTourID { get; set; }
        public int Option { get; set; }
        public int Adult { get; set; }
        public DateTime Departure { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string SocialMedia { get; set; }
        public string Request { get; set; }
        public string PromotionCode { get; set; }
        public string RoomType { get; set; }
        public int TypePayment { get; set; }
        public int Child { get; set; }
        public string Address { get; set; }

    }
}