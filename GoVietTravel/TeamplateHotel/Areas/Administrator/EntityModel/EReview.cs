using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProjectLibrary.Database;
using System;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EReview
    {
        public int ID { get; set; }

        [DisplayName("Obj")]
        [Required(ErrorMessage = "Please select a Obj")]
        public int ObjID { get; set; }

        [DisplayName("Full name")]
        [MaxLength(150, ErrorMessage = "150 characters max")]
        [Required(ErrorMessage = "Please enter a full name")]
        public string FullName { get; set; }

        [DisplayName("Gender")]
        [Required(ErrorMessage = "Please enter a Gender")]
        public bool Gender { get; set; }

        [DisplayName("Title")]
        [MaxLength(100, ErrorMessage = "100 characters max")]
        [Required(ErrorMessage = "Please enter a Title")]
        public string Title { get; set; }

        [DisplayName("Content")]
        [Required(ErrorMessage = "Please enter a Content")]
        public string Content { get; set; }

        [DisplayName("Category Menu")]
        [Required(ErrorMessage = "Please select a Category Menu")]
        public int MenuID { get; set; }

        public int? Index { get; set; }

        [DisplayName("Point")]
        [Required(ErrorMessage = "Please enter a Point")]
        public int Point { get; set; }

        [DisplayName("Time Review")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please enter a Time Review")]
        public DateTime TimeReview { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Please enter a Address")]
        public string Address { get; set; }

        [DisplayName("Profile Images")]
        [MaxLength(500, ErrorMessage = "500 characters max")]
        public string ProfileImages { get; set; }

        [DisplayName("Display Status")]
        [Required(ErrorMessage = "Please enter a DisplayStatus")]
        public bool DisplayStatus { get; set; }

        [DisplayName("Use Service")]
        [Required(ErrorMessage = "Please enter a UseService")]
        public bool UseService { get; set; }

        [DisplayName("Email")]
        [MaxLength(256, ErrorMessage = "256 characters max")]
        [Required(ErrorMessage = "Please enter a Email")]
        public string Email { get; set; }

        [DisplayName("Kind Of Trip")]
        [Required(ErrorMessage = "Please enter a KindOfTrip")]
        public string KindOfTrip { get; set; }

        [DisplayName("ItineraryPoint")]
        [Required(ErrorMessage = "Please enter a ItineraryPoint")]
        public int ItineraryPoint { get; set; }

        [DisplayName("Food Drink Point")]
        [Required(ErrorMessage = "Please enter a Food Drink Point")]
        public int FoodDrinkPoint { get; set; }

        [DisplayName("Guide Point")]
        [Required(ErrorMessage = "Please enter a Guide Point")]
        public int GuidePoint { get; set; }

        [DisplayName("Activity Point")]
        [Required(ErrorMessage = "Please enter a Activity Point")]
        public int ActivityPoint { get; set; }

        [DisplayName("Accomodations Point")]
        [Required(ErrorMessage = "Please enter a Accomodations Point")]
        public int AccomodationsPoint { get; set; }
    }
}