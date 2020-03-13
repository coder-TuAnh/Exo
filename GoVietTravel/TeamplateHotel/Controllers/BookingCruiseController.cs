//using Newtonsoft.Json;
//using ProjectLibrary.Config;
//using ProjectLibrary.Database;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Globalization;
//using System.Linq;
//using System.Reflection;
//using System.Web;
//using System.Web.Mvc;
//using TeamplateHotel.Areas.Administrator.EntityModel;
//using TeamplateHotel.Models;
//using PagedList;

//namespace TeamplateHotel.Controllers
//{
//    public class BookingCruiseController : Controller
//    {

//        private readonly MyDbDataContext db = new MyDbDataContext();
        
//        // tim kiem tour
//        public ActionResult DetailSearch(string namecruise, string chekin, string catalorycruise, string duration, string price, int? page, string mostcruise)
//        {
//            var listcruise = db.Cruises.ToList();

//            List<SelectListItem> listmenu = new List<SelectListItem>();
//            listmenu.Add(new SelectListItem() { Value = "All trip types", Text = "All" });
//            foreach (var b in db.Menus.Where(a => a.Type == SystemMenuType.Cruise).ToList())
//            {
//                listmenu.Add(new SelectListItem() { Value = b.Title, Text = b.ID.ToString() });
//            }
//            ViewBag.ListMenuID = new SelectList(listmenu, "Text", "Value");

//            //List Price
//            List<SelectListItem> listpricemenu = new List<SelectListItem>();
//            listpricemenu.Add(new SelectListItem() { Text = "", Value = "All price" });
//            listpricemenu.Add(new SelectListItem() { Text = "50;100", Value = "50 - 100 USD" });
//            listpricemenu.Add(new SelectListItem() { Text = "100;250", Value = "100 - 250 USD" });
//            listpricemenu.Add(new SelectListItem() { Text = "250;400", Value = "250 - 400 USD" });
//            listpricemenu.Add(new SelectListItem() { Text = "400;5000000", Value = "400 + USD" });
//            ViewBag.ListMenuPrice = new SelectList(listpricemenu, "Text", "Value");

//            //List CruiseMost
//            List<SelectListItem> listmostcruise = new List<SelectListItem>();
//            listmostcruise.Add(new SelectListItem() { Text = "1", Value = "Most popular" });
//            listmostcruise.Add(new SelectListItem() { Text = "2", Value = "Top rated" });
//            listmostcruise.Add(new SelectListItem() { Text = "3", Value = "Prices (Low - High)" });
//            listmostcruise.Add(new SelectListItem() { Text = "4", Value = "Prices (High - Low)" });
//            ViewBag.ListMostCruise = new SelectList(listmostcruise, "Text", "Value");

//            ViewBag.listDay = GetListDay();

//            if (!string.IsNullOrEmpty(namecruise))
//            {
//                listcruise = listcruise.Where(a => a.Name.Contains(namecruise)).ToList();
//            }
//            if (!string.IsNullOrEmpty(catalorycruise) && catalorycruise != "All")
//            {
//                listcruise = listcruise.FindAll(x => x.MenuID != null && x.MenuID.Contains(catalorycruise));
//            }
//            if (!string.IsNullOrEmpty(duration) && duration != "ALL")
//            {
//                listcruise = listcruise.Where(a => a.Duration == duration).ToList();
//            }
//            if (!string.IsNullOrEmpty(price))
//            {
//                var listprice = price.Split(';');
//                listcruise = listcruise.Where(a => a.Price > float.Parse(listprice[0]) && a.Price < float.Parse(listprice[1])).ToList();
//            }
//            if (!string.IsNullOrEmpty(mostcruise))
//            {
//                if (mostcruise == "1")
//                {
//                    listcruise = listcruise.Where(a => a.BestCruise == false).ToList();
//                }
//                else if (mostcruise == "2")
//                {
//                    listcruise = listcruise.Where(a => a.BestCruise == true).ToList();
//                }
//                else if (mostcruise == "3")
//                {
//                    listcruise = listcruise.OrderBy(a => a.Price).ToList();
//                }
//                else if (mostcruise == "4")
//                {
//                    listcruise = listcruise.OrderByDescending(a => a.Price).ToList();
//                }
//            }
//            int pageNumber = (page ?? 1);
//            int pageSize = 10;

//            ViewBag.Namecruise = namecruise;
//            ViewBag.Chekin = chekin;
//            ViewBag.Catalorycruise = catalorycruise;
//            ViewBag.Duration = duration;
//            ViewBag.Price = price;
//            return View(listcruise.ToPagedList(pageNumber, pageSize));
//        }
//        public ActionResult DetailCruise(string alias, int id)
//        {
//            var model = db.Cruises.FirstOrDefault(a => a.ID == id);

//            List<SelectListItem> listmenu = new List<SelectListItem>();
//            listmenu.Add(new SelectListItem() { Value = "All trip types", Text = "All" });
//            foreach (var b in db.Menus.Where(a => a.Type == SystemMenuType.Cruise).OrderBy(a => a.ID).ToList())
//            {
//                listmenu.Add(new SelectListItem() { Value = b.Title, Text = b.ID.ToString() });
//            }
//            ViewBag.ListMenuID = new SelectList(listmenu, "Text", "Value");

//            //List Price
//            List<SelectListItem> listpricemenu = new List<SelectListItem>();
//            listpricemenu.Add(new SelectListItem() { Text = "", Value = "All price" });
//            listpricemenu.Add(new SelectListItem() { Text = "50;100", Value = "50 - 100 USD" });
//            listpricemenu.Add(new SelectListItem() { Text = "100;250", Value = "100 - 250 USD" });
//            listpricemenu.Add(new SelectListItem() { Text = "250;400", Value = "250 - 400 USD" });
//            listpricemenu.Add(new SelectListItem() { Text = "400;5000000", Value = "400 + USD" });
//            ViewBag.ListMenuPrice = new SelectList(listpricemenu, "Text", "Value");

//            //List CruiseMost
//            List<SelectListItem> listmostcruise = new List<SelectListItem>();
//            listmostcruise.Add(new SelectListItem() { Text = "1", Value = "Most popular" });
//            listmostcruise.Add(new SelectListItem() { Text = "2", Value = "Top rated" });
//            listmostcruise.Add(new SelectListItem() { Text = "3", Value = "Prices (Low - High)" });
//            listmostcruise.Add(new SelectListItem() { Text = "4", Value = "Prices (High - Low)" });
//            ViewBag.ListMostCruise = new SelectList(listmostcruise, "Text", "Value");


//            if (model != null)
//            {
//                List<Cruisetab> cruiseTabs = db.Cruisetabs.Where(a => a.IDCruise == model.ID).ToList();
//                List<Cruisetab> cruisesearchTabs = db.Cruisetabs.Where(a => a.IDCruise == model.ID && a.Price > 0).ToList();
//                List<Cabin> cruiceCabin = db.Cabins.Where(a => a.IDCruise == model.ID).ToList();
//                List<Cruise> cruises = db.Cruises.Where(a => a.MenuID == id.ToString()).OrderBy(a => a.ID).ToList();

//                List<SelectListItem> listtabcruise = new List<SelectListItem>();
//                foreach (var b in cruisesearchTabs)
//                {
//                    listtabcruise.Add(new SelectListItem() { Value = b.Name, Text = b.Name });
//                }
//                ViewBag.ListTabCruise = new SelectList(listtabcruise, "Text", "Value");

//                //foreach (var item in tours)
//                //{
//                //    item.MenuAlias = item.Menu.Alias;
//                //}
//                List<CruiseGallery> listDetail = new List<CruiseGallery>();
//                listDetail = JsonConvert.DeserializeObject<List<CruiseGallery>>(model.Cruisegallery);
//                DetailCruise detailcCruise = new DetailCruise()
//                {
//                    CruiseCabin = cruiceCabin,
//                    Cruise = model,
//                    Cruises = cruises,
//                    Cruisetabs = cruiseTabs,
//                    cruiseGallery = listDetail
//                };

//                return View(detailcCruise);
//            }
//            return RedirectToAction("Index");
//        }

       
//        public JsonResult listcabinofcruise(string id)
//        {
//            int ID = int.Parse(id.Trim());
//            List<Cabin> list = db.Cabins.Where(a => a.IDCruise == ID).ToList();
            
//            var lst = list.Select(a => new
//            {
//              a.ID,
//              a.IDCruise,
//              a.Name,
//              a.Price,
//              a.Size,
//              a.MaxAdults,
//              a.Bed,
//              a.Description,
//              a.Content,
//              a.Image,
//              a.Cabingallery,
//            });
//            return Json(new
//            {
//                data = lst
//            }, JsonRequestBehavior.AllowGet);
//        }
//        #region BookCruise
//        // đặt phòng
//        public ActionResult DetaiBookingCruise()
//        {
           
//            try
//            {
//                BookingCruise ds = (BookingCruise)Session["detaiboookingcruise"];

//                double total = 0;
//                if (ds.CruiseCabin.Count > 0)
//                {
//                    foreach (var i in ds.CruiseCabin)
//                    {
//                        var coutrom = ds.listcabin.FirstOrDefault(a => a.IDCabin == i.ID).CountRom;
//                        // kiem tra xem phong do co code giam gia ko
//                        var code = ds.listcabin.FirstOrDefault(a => a.IDCabin == i.ID).isPromotionCode;
//                        if (!string.IsNullOrEmpty(i.Price))
//                        {
//                            var listtab = JsonConvert.DeserializeObject<List<EPricetabCabin>>(i.Price);
//                            var pricerome = listtab.FirstOrDefault(a => a.NameTabCruise == ds.Duration);
//                            if (pricerome != null)
//                            {
//                                //total += coutrom * pricerome.Price;
//                                if (code == true)
//                                {
//                                    total += coutrom * pricerome.Pricesale;
//                                }
//                                else
//                                {
//                                    total += coutrom * pricerome.Price;
//                                }
//                            }
//                        }


//                    }
//                }
//                ds.listservic = db.ServiceCruises.ToList();
//                ViewBag.totalrom = total;
//                ViewBag.CountRoms = ds.listcabin.Sum(a => a.CountRom);
//                return View(ds);
//            }
//            catch
//            {
//                return View("Index");
//            }

//        }
//        [HttpPost]
//        public ActionResult DetaiBookingCruise(BookingCruise ds)
//        {
//            var listcabin = new List<Cabin>();
//            foreach (var c in ds.listcabin)
//            {
//                var model = db.Cabins.FirstOrDefault(a => a.ID == c.IDCabin);
//                listcabin.Add(model);
//            }
//            ds.CruiseCabin = listcabin;
//            ds.Cruise = db.Cruises.FirstOrDefault(a => a.ID == ds.IDCruise);
//            Session["detaiboookingcruise"] = ds;
//            return Json(new { success = true });
//        }
//        #endregion
//        // kiem tra ma giam gia

//        [HttpPost]
//        public JsonResult Cormfirmbookingcruise(string name, string email, string phone, string address, string country, string mrs, string content, List<BookingCabin> listservic)
//        {
//            try
//            {
//                BookingCruise ds = (BookingCruise)Session["detaiboookingcruise"];
//                var listcabinemial = "";
//                var listserviceemial = "";
//                double PriceCabin = 0;
//                double PriceService = 0;
//                if (ds.CruiseCabin.Count > 0)
//                {
//                    for (var i = 0; i < ds.CruiseCabin.Count; i++)
//                    {
//                        var element = ds.CruiseCabin[i];
//                        var coutrom = ds.listcabin.FirstOrDefault(a => a.IDCabin == element.ID).CountRom;
//                        if (!string.IsNullOrEmpty(element.Price))
//                        {
//                            var listtab = JsonConvert.DeserializeObject<List<EPricetabCabin>>(element.Price);
//                            var pricerome = listtab.FirstOrDefault(a => a.NameTabCruise == ds.Duration);
//                            if (pricerome != null)
//                            {
//                                if (ds.listcabin[i].isPromotionCode == true)
//                                {
//                                    PriceCabin += coutrom * pricerome.Pricesale;
//                                }
//                                else
//                                {
//                                    PriceCabin += coutrom * pricerome.Price;
//                                }

//                                listcabinemial += "+Room " + (i + 1) + ": " + element.Name + "<br> Number of cabin: " + coutrom + "<br />";
//                            }
//                        }
//                    }
//                }
//                if (listservic != null)
//                {
//                    foreach (var i in listservic)
//                    {
//                        var countservic = db.ServiceCruises.FirstOrDefault(a => a.ID == i.IDCabin);
//                        if (countservic != null)
//                        {
//                            PriceService += (countservic.Price ?? 0) * i.CountRom;
//                            listserviceemial += "+ " + countservic.Name + ":<br>number of guest: " + i.CountRom + "<br>+Price :" + (countservic.Price ?? 0) * i.CountRom + " USD <br />";
//                        }
//                    }
//                }
//                var cruisebooking = new BookCruise
//                {
//                    PriceCabin = PriceCabin,
//                    FullName = name,
//                    Address = address,
//                    Request = content,
//                    Tel = phone,
//                    Gender = mrs,
//                    Email = email,
//                    Adult = ds.Adult,
//                    Country = country,
//                    Child = Int32.Parse(ds.Child),
//                    CreateDate = DateTime.Now,
//                    Departure = ds.Checkin,
//                    NameCruise = ds.Cruise.Name,
//                    Itinerary = ds.Duration,
//                    PriceService = PriceService,
//                    Total = PriceService + PriceCabin
//                };
//                cruisebooking.InforService = listserviceemial;
//                cruisebooking.InforCabin = listcabinemial;
//                db.BookCruises.InsertOnSubmit(cruisebooking);
//                //db.SubmitChanges();

//                //Gửi email xác nhận đặt Tàu
//                Hotel hotel = CommentController.DetailHotel(Request.Cookies["LanguageID"].Value);
//                SendEmail sendEmail = db.SendEmails.FirstOrDefault(a => a.Type == TypeSendEmail.BookCruise && a.LanguageID == Request.Cookies["LanguageID"].Value);

//                sendEmail.Title = "Title Email Booking Cruise"; /*sendEmail.Title.Replace("{HotelName}", hotel.Name);*/
//                string contentemail = sendEmail.Content;
//                sendEmail.Title = sendEmail.Title.Replace("{HotelName}", hotel.Name);
//                contentemail = contentemail.Replace("{NameCruise}", cruisebooking.NameCruise);
//                contentemail = contentemail.Replace("{Gender}", cruisebooking.Gender);
//                contentemail = contentemail.Replace("{FullName}", cruisebooking.FullName);
//                contentemail = contentemail.Replace("{Email}", cruisebooking.Email);
//                contentemail = contentemail.Replace("{Tel}", cruisebooking.Tel);
//                contentemail = contentemail.Replace("{Address}", cruisebooking.Address);
//                contentemail = contentemail.Replace("{City}", "No");
//                contentemail = contentemail.Replace("{Country}", cruisebooking.Country);
//                contentemail = contentemail.Replace("{Smoking}", "Something");
//                contentemail = contentemail.Replace("{InforService}", listserviceemial);
//                contentemail = contentemail.Replace("{InforCabin}", listcabinemial);
//                contentemail = contentemail.Replace("{Request}", cruisebooking.Request);
//                contentemail = contentemail.Replace("{CheckIn}", cruisebooking.Departure);
//                contentemail = contentemail.Replace("{Itinerary}", ds.Duration);
//                contentemail = contentemail.Replace("{Adult}", cruisebooking.Adult.ToString());
//                contentemail = contentemail.Replace("{Child}", cruisebooking.Child.ToString());
//                contentemail = contentemail.Replace("{PriceCabin}", cruisebooking.PriceCabin.ToString());
//                contentemail = contentemail.Replace("{PriceService}", cruisebooking.PriceService.ToString());
//                contentemail = contentemail.Replace("{Total}", cruisebooking.Total.ToString());
//                contentemail = contentemail.Replace("{HotelName}", hotel.Name);
//                contentemail = contentemail.Replace("{HotelEmail}", hotel.Email);
//                contentemail = contentemail.Replace("{HotelTel}", hotel.Tel);
//                contentemail = contentemail.Replace("{Website}", hotel.Website);
//                MailHelper.SendMail(cruisebooking.Email, sendEmail.Title, contentemail);
//                //MailHelper.SendMail(hotel.Email, hotel.Name + " (113)- Booking Cruise of " + cruisebooking.FullName, contentemail);

//                return Json(new { success = true });
//            }
//            catch
//            {
//                return Json(new { success = false });
//            }
//        }

//        public ActionResult MessageThank()
//{
//    return View();
//}

//        [HttpPost]
//        public JsonResult CheckMa(string code, int IDCabin)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    DateTime today = DateTime.Now;
//                    ProjectLibrary.Database.PromotionCodeCabin del = db.PromotionCodeCabins.FirstOrDefault(c => c.Code == code && c.IDCabin == IDCabin
//                    && today <= c.NgayKetThuc && today >= c.NgayBatDau && c.TrangThai == true);
//                    if (del != null)
//                    {
//                        /*
//                         1-het ma
//                         2-ok co ma va con luot dung
//                         3-loi ko co ma nao
//                         */
//                        if (del.DaDung >= del.SL)
//                        {
//                            return Json(new { Result = "OK", Message = "1" });
//                        }
//                        else
//                        {
//                            del.DaDung = del.DaDung + 1;
//                            db.SubmitChanges();
//                            return Json(new { Result = "OK", Message = "2" });
//                        }

//                    }
//                    return Json(new { Result = "ERROR", Message = "3" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }

//        private SelectList GetListDay()
//        {
//            var list = new List<object>();
//            list.Add(new { name = "ALL", display = "All Duration" });
//            foreach (FieldInfo fieldInfo in typeof(Duration).GetFields())
//            {
//                if (fieldInfo.FieldType.Name != "Duration")
//                    continue;
//                var attribute = Attribute.GetCustomAttribute(fieldInfo,
//                    typeof(DisplayAttribute)) as DisplayAttribute;

//                list.Add(attribute != null
//                    ? new { name = fieldInfo.Name, display = attribute.Name }
//                    : new { name = fieldInfo.Name, display = fieldInfo.Name });
//            }
//            return new SelectList(list, "name", "display");
//        }

        
//    }
//}
