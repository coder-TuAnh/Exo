using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using TeamplateHotel.Models;
using Newtonsoft.Json;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Controllers
{
    public class BookController : Controller
    {
        private readonly MyDbDataContext db = new MyDbDataContext();
        // đặt phòng
        public ActionResult DetaiBookingCruise()
        {
            try
            {
                BookingCruise ds = (BookingCruise)Session["detaiboookingcruise"];

                double total = 0;
                if (ds.CruiseCabin.Count > 0)
                {
                    foreach (var i in ds.CruiseCabin)
                    {
                        var coutrom = ds.listcabin.FirstOrDefault(a => a.IDCabin == i.ID).CountRom;
                        // kiem tra xem phong do co code giam gia ko
                        var code = ds.listcabin.FirstOrDefault(a => a.IDCabin == i.ID).isPromotionCode;
                        if (!string.IsNullOrEmpty(i.Price))
                        {
                            var listtab = JsonConvert.DeserializeObject<List<EPricetabCabin>>(i.Price);
                            var pricerome = listtab.FirstOrDefault(a => a.NameTabCruise == ds.Duration);
                            if (pricerome != null)
                            {
                                //total += coutrom * pricerome.Price;
                                if (code == true)
                                {
                                    total += coutrom * pricerome.Pricesale;
                                }
                                else
                                {
                                    total += coutrom * pricerome.Price;
                                }
                            }
                        }
                    }
                }
                ds.listservic = db.ServiceCruises.ToList();
                ViewBag.totalrom = total;
                ViewBag.CountRoms = ds.listcabin.Sum(a => a.CountRom);
                return View(ds);
            }
            catch
            {
                return View("Index");
            }

        }
        [HttpPost]
        public ActionResult DetaiBookingCruise(BookingCruise ds)
        {
            var listcabin = new List<Cabin>();
            foreach (var c in ds.listcabin)
            {
                var model = db.Cabins.FirstOrDefault(a => a.ID == c.IDCabin);
                listcabin.Add(model);
            }
            ds.CruiseCabin = listcabin;
            ds.Cruise = db.Cruises.FirstOrDefault(a => a.ID == ds.IDCruise);
            Session["detaiboookingcruise"] = ds;
            return Json(new { success = true });
        }
        [HttpPost]
        // code gửi mail xacs nhận booking 
        public JsonResult Cormfirmbookingcruise(string name, string email, string phone, string address, string country, string mrs, string content, List<BookingCabin> listservic)
        {
            try
            {
                BookingCruise ds = (BookingCruise)Session["detaiboookingcruise"];
                var listcabinemial = "";
                var listserviceemial = "";
                double PriceCabin = 0;
                double PriceService = 0;
                if (ds.CruiseCabin.Count > 0)
                {
                    for (var i = 0; i < ds.CruiseCabin.Count; i++)
                    {
                        var element = ds.CruiseCabin[i];
                        var coutrom = ds.listcabin.FirstOrDefault(a => a.IDCabin == element.ID).CountRom;
                        if (!string.IsNullOrEmpty(element.Price))
                        {
                            var listtab = JsonConvert.DeserializeObject<List<EPricetabCabin>>(element.Price);
                            var pricerome = listtab.FirstOrDefault(a => a.NameTabCruise == ds.Duration);
                            if (pricerome != null)
                            {
                                if (pricerome.Pricesale < pricerome.Price)
                                {
                                    PriceCabin += coutrom * pricerome.Pricesale;
                                }
                                else
                                {
                                    PriceCabin += coutrom * pricerome.Price;
                                }

                                listcabinemial += "+Room " + (i + 1) + ": " + element.Name + "<br> Number of cabin: " + coutrom + "<br />";
                            }
                        }
                    }
                }
                if (listservic != null)
                {
                    foreach (var i in listservic)
                    {
                        var countservic = db.ServiceCruises.FirstOrDefault(a => a.ID == i.IDCabin);
                        if (countservic != null)
                        {
                            PriceService += (countservic.Price ?? 0) * i.CountRom;
                            listserviceemial += "+ " + countservic.Name + ":<br>number of guest: " + i.CountRom + "<br>+Price :" + (countservic.Price ?? 0) * i.CountRom + " USD <br />";
                        }
                    }
                }
                var cruisebooking = new BookCruise
                {
                    PriceCabin = PriceCabin,
                    FullName = name,
                    Address = address,
                    Request = content,
                    Tel = phone,
                    Gender = mrs,
                    Email = email,
                    Adult = ds.Adult,
                    Country = country,
                    Child = Int32.Parse(ds.Child),
                    CreateDate = DateTime.Now,
                    Departure = ds.Checkin,
                    NameCruise = ds.Cruise.Name,
                    Itinerary = ds.Duration,
                    PriceService = PriceService,
                    Total = PriceService + PriceCabin
                };
                cruisebooking.InforService = listserviceemial;
                cruisebooking.InforCabin = listcabinemial;
                db.BookCruises.InsertOnSubmit(cruisebooking);
                db.SubmitChanges();

                //Gửi email xác nhận đặt Tàu

                Hotel hotel = CommentController.DetailHotel(Request.Cookies["LanguageID"].Value);
                SendEmail sendEmail =
                db.SendEmails.FirstOrDefault(
                    a => a.Type == TypeSendEmail.BookRoom && a.LanguageID == Request.Cookies["LanguageID"].Value);

                sendEmail.Title = sendEmail.Title.Replace("{HotelName}", hotel.Name);
                string contentemail = sendEmail.Content;
                contentemail = contentemail.Replace("{NameCruise}", cruisebooking.NameCruise);
                contentemail = contentemail.Replace("{Gender}", cruisebooking.Gender);
                contentemail = contentemail.Replace("{FullName}", cruisebooking.FullName);
                contentemail = contentemail.Replace("{Email}", cruisebooking.Email);
                contentemail = contentemail.Replace("{Tel}", cruisebooking.Tel);
                contentemail = contentemail.Replace("{Address}", cruisebooking.Address);
                contentemail = contentemail.Replace("{City}", "No");
                contentemail = contentemail.Replace("{Country}", cruisebooking.Country);
                contentemail = contentemail.Replace("{Smoking}", "Something");
                contentemail = contentemail.Replace("{InforService}", listserviceemial);
                contentemail = contentemail.Replace("{InforCabin}", listcabinemial);
                contentemail = contentemail.Replace("{Request}", cruisebooking.Request);
                contentemail = contentemail.Replace("{Request}", cruisebooking.Request);
                contentemail = contentemail.Replace("{CheckIn}", cruisebooking.Departure);
                contentemail = contentemail.Replace("{Itinerary}", ds.Duration);
                contentemail = contentemail.Replace("{Adult}", cruisebooking.Adult.ToString());
                contentemail = contentemail.Replace("{Child}", cruisebooking.Child.ToString());
                contentemail = contentemail.Replace("{PriceCabin}", cruisebooking.PriceCabin.ToString());
                contentemail = contentemail.Replace("{PriceService}", cruisebooking.PriceService.ToString());
                contentemail = contentemail.Replace("{Total}", cruisebooking.Total.ToString());
                contentemail = contentemail.Replace("{HotelName}", hotel.Name);
                contentemail = contentemail.Replace("{HotelEmail}", hotel.Email);
                contentemail = contentemail.Replace("{HotelTel}", hotel.Tel);
                contentemail = contentemail.Replace("{Website}", hotel.Website);

                MailHelper.SendMail(cruisebooking.Email, sendEmail.Title, contentemail);
                MailHelper.SendMail(hotel.Email, hotel.Name + " (113)- Booking Cruise of " + cruisebooking.FullName, contentemail);
                
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}