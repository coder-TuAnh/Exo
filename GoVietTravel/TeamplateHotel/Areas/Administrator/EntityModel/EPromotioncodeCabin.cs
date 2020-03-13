using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EPromotioncodeCabin
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int SL { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public bool TrangThai { get; set; }
        public int DaDung { get; set; }
        public string MoTa { get; set; }
        public int IDCabin { get; set; }

    }
}