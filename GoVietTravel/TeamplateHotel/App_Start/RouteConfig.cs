using System.Web.Mvc;
using System.Web.Routing;

namespace TeamplateHotel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "get-rv",
                  "load-review",
                   new { controller = "BookingTour", action = "LoadDataReview", id = UrlParameter.Optional },
                   new[] { "TeamplateHotel.Controllers" }
             );
            ////customize-your-trip
            routes.MapRoute(
             name: "customize-your-trip",
             url: "customize-your-trip/{action}/{id}",
             defaults: new { controller = "CTrip", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute("Ctrip", "customize-trip", new
            {
                controller = "CTrip",
                action = "Index",
            });
            ////Visa
            routes.MapRoute("Visa", "visa-vietnam", new
            {
                controller = "Visa",
                action = "index",
            });

            routes.MapRoute("Visa-default", "visa-vietnam/{action}", new
            {
                controller = "Visa",
                action = "index",
            });

            routes.MapRoute("Booking2", "Booking/SendBooking", new
            {
                controller = "Booking",
                action = "SendBooking",
            });
            routes.MapRoute("Booking3", "Booking/Messages", new
            {
                controller = "Booking",
                action = "Messages",
            });
            //booking room
            routes.MapRoute("Booking1", "Booking", new
            {
                controller = "Booking",
                action = "MakeReservation",
            });

            //contact
            routes.MapRoute("Contact", "Contact/SubmitContact", new
            {
                controller = "SendContact",
                action = "SubmitContact",
            });
            //contact
            routes.MapRoute("Contact Messages", "Contact/Messages", new
            {
                controller = "SendContact",
                action = "Messages",
            });

            //Booking tour
           

            routes.MapRoute("BookTour1", "BookTour/SendBooking", new
            {
                controller = "BookingTour",
                action = "SendBooking",
            });
            routes.MapRoute("BookTour2", "BookTour/Messages", new
            {
                controller = "BookingTour",
                action = "Messages",
            });
            routes.MapRoute("CheckCode", "BookTour/CheckCode", new
            {
                controller = "BookingTour",
                action = "CheckCode",
            });

            routes.MapRoute("Submit-InvoidOnePay", "BookTour/SubmitInvoidOnePay", new
            {
                controller = "BookingTour",
                action = "SubmitInvoidOnePay",
            });
            routes.MapRoute("Message-OnePay", "BookTour/MessageOnePay", new
            {
                controller = "BookingTour",
                action = "MessageOnePay",
            });

            //Review tour
            routes.MapRoute("Review tour", "Review-tour", new
            {
                controller = "BookingTour",
                action = "AddReview",
            });
            //DetailSearch
            routes.MapRoute(
              "Cruise Search",
              "BookingCruise/DetailSearch/{id}",
               new { controller = "BookingCruise", action = "DetailSearch", id = UrlParameter.Optional },
              new[] { "TeamplateHotel.Controllers" }
           );

            // Boking Cruise
            routes.MapRoute(
                 "Cruise Detail",
                 "cruise-detail/{alias}-{id}",
                new { controller = "BookingCruise", action = "DetailCruise", id = UrlParameter.Optional },
                new[] { "TeamplateHotel.Controllers" }
                );
            routes.MapRoute(
               name: "Mesage thank",
               url: "mesage-thank-for-you",
               defaults: new { controller = "BookingCruise", action = "MessageThank", UrlParameter.Optional },
               namespaces: new[] { "TeamplateHotel.Controllers" }
            );
            routes.MapRoute(
              name: "Cruise liscabin",
              url: "BookingCruise/listcabinofcruise/{id}",
              defaults: new { controller = "BookingCruise", action = "listcabinofcruise", id = UrlParameter.Optional },
              namespaces: new[] { "TeamplateHotel.Controllers" }
           );
            routes.MapRoute(// xac nhan gui mail
               name: "Cruise booking",
               url: "BookingCruise/Cormfirmbookingcruise",
               defaults: new { controller = "BookingCruise", action = "Cormfirmbookingcruise", id = UrlParameter.Optional },
                namespaces: new[] { "TeamplateHotel.Controllers" }
            );
            // xac nhan ma giam gia
            routes.MapRoute(
                name: "check PromotionCode Cabin",
                url: "Check-Promotioncode-Cabin",
                defaults: new { Controller = "BookingCruise", Action = "CheckMa" },
                namespaces: new[] { "TeamplateHotel.Controllers" }
            );
            //Booking tour
            routes.MapRoute("BookTour3", "BookTour/{id}/{alias}", new
            {
                controller = "BookingTour",
                action = "BookTour",
                id = UrlParameter.Optional,
                alias = UrlParameter.Optional
            });
            routes.MapRoute(
               name: "Cruisedetail booking",
               url: "BookingCruise/DetaiBookingCruise",
               defaults: new { controller = "BookingCruise", action = "DetaiBookingCruise", id = UrlParameter.Optional },
               namespaces: new[] { "TeamplateHotel.Controllers" }
            );
            routes.MapRoute(
               name: "EmailMarketing",
               url: "EmailMarketing/SaveEmail",
               defaults:
                   new
                   {
                       controller = "EmailMarketing",
                       action = "SaveEmail",
                   });
            routes.MapRoute(
              name: "FeedBack1",
              url: "FeedBack/SaveEmail",
              defaults:
                  new
                  {
                      controller = "FeedBack",
                      action = "SaveEmail",
                  });
            routes.MapRoute("Default", "{aliasMenuSub}/{idSub}/{aliasSub}", new
            {
                controller = "Home",
                action = "Index",
                aliasMenuSub = UrlParameter.Optional,
                idSub = UrlParameter.Optional,
                aliasSub = UrlParameter.Optional
            });
        }
    }
}