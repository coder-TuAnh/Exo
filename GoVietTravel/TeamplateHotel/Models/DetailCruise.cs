using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Models
{
   
    public class DetailCruise
    {
        public Cruise Cruise { get; set; }
        public List<CruiseGallery> cruiseGallery { get; set; }
        public List<Cruise> Cruises { get; set; }
        public List<Cruisetab> Cruisetabs { get; set; }
        public List<Cabin> CruiseCabin { get; set; }
    }
    public class BookingCruise
    {
        public int IDCruise { get; set; }//mã cabin
        public Cruise Cruise { get; set; }
        public int Adult { get; set; }
        public string Duration { get; set; }
        public string Checkin { get; set; }
        public string Child { get; set; }
        public List<ServiceCruise> listservic { get; set; }
        public List<Cabin> CruiseCabin { get; set; }
        public List<BookingCabin> listcabin { get; set; }
    }
    public class BookingCabin
    {
        public int IDCabin { get; set; }//mã cabin
        public int CountRom { get; set; } // số phòng
        public bool isPromotionCode { get; set; } // co ma giam gia hay ko
    }
    public class JsoneLikeCruise
    {
        public int IDCruise { get; set; }//mã Cruise

    }
}