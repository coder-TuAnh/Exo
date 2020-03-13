using System;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EPromotionCode
    {
        public int ID { get; set; }

        [Required]
        public int IDTour { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public int Total { get; set; }

        [Required]
        public int Used { get; set; }

        [Required]
        public DateTime StartDay { get; set; }

        [Required]
        public DateTime EndDay { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public double Rate { get; set; }

        public string Description { get; set; }
    }
}