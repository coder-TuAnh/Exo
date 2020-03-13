using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EProcessingTimeVisa
    {
        public int ID { get; set; }

        [DisplayName("Tên ")]
        [Required(ErrorMessage = "Vui lòng điền vào trường này")]
        public string Name { get; set; }
        
        [DisplayName("Giá Loại")]
        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public int Price { get; set; }

    }
}