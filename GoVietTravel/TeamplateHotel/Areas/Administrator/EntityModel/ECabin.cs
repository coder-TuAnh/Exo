using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ECabin
    {
        public int ID { get; set; }

        public int IDCruise { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string Pricechildren { get; set; }

        public double Size { get; set; }

        public int MaxAdults { get; set; }

        public string Bed { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public string Pricesale { get; set; }

        public string Cabingallery { get; set; }

        public List<EPricetabCabin> listcruitab { get; set; }
    }
    public class CaBinGallery
    {
        public string NameImages { get; set; }
    }
    public class EPricetabCabin
    {
        public int IDTabCruise { get; set; }
        public string NameTabCruise { get; set; }
        public float Price { get; set; }
        public float Pricechildren { get; set; }
        public float Pricesale { get; set; }
    }
}