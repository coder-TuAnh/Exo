using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectLibrary.Database;
namespace TeamplateHotel.Models
{
    public class ECustomTrip
    {
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public DateTime DepartureDate { get; set; }
        public bool NeedVNVisa { get; set; }
        public string OtherRequest { get; set; }
        public string Describe { get; set; }
    }
}