using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using System.Web;
using System.IO;
using ProjectLibrary.Utility;
using TeamplateHotel.Models;
using TeamplateHotel.Handler;
using System.Xml;

namespace TeamplateHotel.Controllers
{
    public class BookingTourController : Controller
    {
        // GET: /BookingTour/
        [HttpGet]
        public ActionResult BookTour(int id)
        {
            using (var db = new MyDbDataContext())
            {
                var bookTour = new BookTour();
                Tour tour = db.Tours.FirstOrDefault(a => a.ID == id);
                if (tour == null)
                {
                    var hotel = CommentController.DetailHotel(Request.Cookies["LanguageID"].Value);
                    ViewBag.Messages = "Error! Can not find the selected tous. For more information, please contact us by email: <a href='mailto:" + hotel.Email + "'>" + hotel.Email + "</a> or phone number: < href='tel:" + hotel.Tel + "'>" + hotel.Tel + "</a>";
                    return View("Messages");
                }
                int caseOption = 0;
                string name = "";
                decimal price = 0;
                List<TabTour> tabtours = new List<TabTour>();

                if (tour.HotelService == true)
                {
                    var option = Request.Params["option"];
                    int.TryParse(option, out caseOption);

                    switch (caseOption)
                    {
                        case 1:
                            name = "3-Star Hotels - " + tour.Title;
                            price = (decimal)tour.ThreeStar;
                            break;

                        case 2:
                            name = "4-Star Hotels - " + tour.Title;
                            price = (decimal)tour.FourStar;
                            break;

                        case 3:
                            name = "5-Star Hotels - " + tour.Title;
                            price = (decimal)tour.FiveStar;
                            break;

                        default:
                            var hotel = CommentController.DetailHotel(Request.Cookies["LanguageID"].Value);
                            ViewBag.Messages = "Error! Can not find the selected tous. For more information, please contact us by email: <a href='mailto:" + hotel.Email + "'>" + hotel.Email + "</a> or phone number: < href='tel:" + hotel.Tel + "'>" + hotel.Tel + "</a>";
                            return View("Messages");
                    }
                    var tabtour = new TabTour
                    {
                        TourID=0,
                        TitleTab = name,
                        Price = price
                    };
                    tabtours.Add(tabtour);
                }
                else
                {
                    var listTabtours = db.TabTours.Where(a => a.TourID == tour.ID && a.Price > 0).ToList();
                    if (listTabtours.Count > 0)
                    {
                        tabtours.AddRange(listTabtours);
                        if (listTabtours.Count > 1)
                        {
                            price = 0;
                        }
                        else
                        {
                            price = (decimal)listTabtours[0].Price;
                        }
                    }
                    else
                    {
                        var tabtour = new TabTour
                        {
                           TourID = 0,
                            TitleTab = tour.Title,
                            Price = tour.PriceSale
                        };
                        tabtours.Add(tabtour);
                    }

                }
                ViewBag.Tour = tour;
                ViewBag.title = tour.Title;
                ViewBag.tourID = tour.ID;
                ViewBag.name = name;
                ViewBag.price = price;
                ViewBag.Option = caseOption;
                bookTour.InfoBooking = tour.Title;
                bookTour.ID = tour.ID;

                ViewBag.TabTours = tabtours;
                return View("BookTour", bookTour);
            }
        }

        [HttpPost]
        public ActionResult SendBooking(MBookTour model)
        {
            string status = "success";
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Tour tour = db.Tours.FirstOrDefault(a => a.ID == model.TourId);
                    Hotel hotel = CommentController.DetailHotel(Request.Cookies["LanguageID"].Value);

                    SendEmail sendEmail =
                           db.SendEmails.FirstOrDefault(
                               a => a.Type == TypeSendEmail.BookTour );
                    if (tour == null)
                    {
                        ViewBag.Messages = sendEmail.Error;
                        return View("Messages");
                    }

                    //xác định xem tour có phải là option không
                    string inforBooking = "";
                    decimal price = 0;
                    if (tour.HotelService == true)
                    {
                        switch (model.Option)
                        {
                            case 1:
                                inforBooking = "3-Star Hotels - " + tour.Title;
                                price = (decimal)tour.ThreeStar;
                                break;

                            case 2:
                                inforBooking = "4-Star Hotels - " + tour.Title;
                                price = (decimal)tour.FourStar;
                                break;

                            case 3:
                                inforBooking = "5-Star Hotels - " + tour.Title;
                                price = (decimal)tour.FiveStar;
                                break;

                            default:
                                ViewBag.Messages = sendEmail.Error;
                                return View("Messages");
                        }
                    }
                    else
                    {
                        #region
                        ////xác định lịch trình
                        var tabTour = db.TabTours.FirstOrDefault(a => a.TourID == model.TabTourID);
                        if (tabTour == null)
                        {
                            inforBooking = tour.Title;
                            price = (decimal)tour.PriceSale;
                        }
                        else
                        {
                            if (db.TabTours.Where(a => a.TourID == model.TourId).Count() > 0)
                            {
                                inforBooking = tour.Title + ", Itinerariy: " + tabTour.TitleTab;
                            }
                            else
                            {
                                inforBooking = tour.Title;
                            }
                            price = (decimal)tabTour.Price;
                        }

                        #endregion
                    }
                    //Check Promotion code

                    double rate = 0;
                    DateTime today = DateTime.Now;
                    if (string.IsNullOrEmpty(model.PromotionCode) == false)
                    {
                        PromotionCode Pcode = db.PromotionCodes.FirstOrDefault(c => c.Code == model.PromotionCode && today <= c.EndDay && today >= c.StartDay && c.Status == true);

                        if (Pcode != null)
                        {
                            /*
                             1-het ma
                             2-ok co ma va con luot dung
                             3-loi ko co ma nao
                             */
                            if (Pcode.Used >= Pcode.Total)
                            {
                                rate = 0;
                            }
                            else
                            {
                                rate = Pcode.Rate / 100;
                                Pcode.Used = Pcode.Used + 1;
                                db.SubmitChanges();
                            }
                        }
                    }


                    //tính giá tour

                    price = price * model.Adult - (price * model.Adult * (decimal)0.6);
                    decimal deposit = 0;
                    decimal Balance = 0;

                    if (model.TypePayment == 1)
                    {
                        deposit = price * ((decimal)tour.Deposit / 100);
                    }
                    Balance = price - deposit;

                    string codeBooking = hotel.CodeBooking + "1";
                    if (db.BookTours.Any())
                    {
                        codeBooking = hotel.CodeBooking + db.BookTours.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1;
                    }

                    BookTour bookTour = new BookTour();
                    bookTour.Departure = model.Departure;
                    bookTour.Code = codeBooking;
                    bookTour.CreateDate = DateTime.Now;
                    bookTour.Gender = model.Gender;
                    bookTour.FullName = model.FullName;
                    //bookTour.SocialMedia = model.SocialMedia;
                    bookTour.Tel = model.Tel;
                    bookTour.RoomType = model.RoomType;
                    bookTour.Country = model.Country;
                    bookTour.Email = model.Email;
                    bookTour.Request = model.Request;
                    bookTour.Adult = model.Adult;
                    bookTour.InfoBooking = inforBooking;
                    bookTour.Total = (float)price;
                    //bookTour.Deposit = (float)deposit;
                    bookTour.Balance = (float)Balance;
                    bookTour.TourId = model.TourId;

                    db.BookTours.InsertOnSubmit(bookTour);
                    db.SubmitChanges();

                    sendEmail.Title = sendEmail.Title.Replace("{HotelName}", hotel.Name);
                    string content = sendEmail.Content;
                    content = content.Replace("{HotelName}", hotel.Name);
                    content = content.Replace("{Deposit}", deposit.ToString());
                    content = content.Replace("{Balance}", Balance.ToString());
                    content = content.Replace("{Code}", bookTour.Code);
                    content = content.Replace("{Departure}", bookTour.Departure.ToString());
                    content = content.Replace("{InfoBooking}", bookTour.InfoBooking);
                    content = content.Replace("{Adult}", bookTour.Adult.ToString());
                    content = content.Replace("{TitleTour}", tour.Title);
                    content = content.Replace("{Price}", price.ToString());
                    content = content.Replace("{Gender}", model.Gender);
                    content = content.Replace("{FullName}", model.FullName);
                    content = content.Replace("{SocialMedia}", model.SocialMedia);
                    content = content.Replace("{Tel}", model.Tel);
                    content = content.Replace("{RoomType}", model.RoomType);
                    content = content.Replace("{Country}", model.Country);
                    content = content.Replace("{Email}", model.Email);
                    content = content.Replace("{Request}", model.Request);

                    if (rate != 0)
                    {
                        content = content.Replace("{Promotion}", model.PromotionCode + "/Rate: " + rate * 100 + " %");
                    }
                    else
                    {
                        content = content.Replace("{Promotion}", "No");
                    }
                    content = content.Replace("{Request}", model.Request);

                    //infor Hotel
                    content = content.Replace("{Add}", hotel.Address);
                    content = content.Replace("{Hotline}", hotel.Hotline);
                    content = content.Replace("{EmailHotel}", hotel.Email);
                    content = content.Replace("{Website}", hotel.Website);

                    MailHelper.SendMail(model.Email, sendEmail.Title, content);
                    MailHelper.SendMail(hotel.Email, hotel.Name + " (" + bookTour.Code + ")- Booking tour of " + model.FullName, content);

                    if (model.TypePayment == 1)
                    {
                        price = price * ((decimal)tour.Deposit / 100);
                        return RedirectToAction("SubmitInvoidOnePay",
                       new
                       {
                           idOrder = bookTour.Code,
                           deposit = (Math.Round(price * (decimal)CodeHelper.GetExrate(), 0)) * 100
                       });
                    }
                    else
                    {
                        return Redirect("/BookTour/Messages?status=" + status);
                    }
                }
            }
            catch (Exception ex)
            {
                status = "error";
            }

            return Redirect("/BookTour/Messages?status=" + status);
        }

        [HttpGet]
        public ActionResult Messages()
        {
            using (var db = new MyDbDataContext())
            {
                SendEmail sendEmail =
                       db.SendEmails.FirstOrDefault(
                           a => a.Type == TypeSendEmail.BookTour && a.LanguageID == Request.Cookies["LanguageID"].Value);

                string status = Request.Params["status"];
                ViewBag.Messages = "";
                if (string.IsNullOrEmpty(status) == false)
                {
                    if (status.Equals("success"))
                    {
                        ViewBag.Messages = sendEmail.Success;
                    }
                    else
                    {
                        ViewBag.Messages = sendEmail.Error;
                    }
                }
                else
                {
                    ViewBag.Messages = sendEmail.Error;
                }
                return View();
            }
        }

        [HttpPost]
        public JsonResult CheckCode(string code, int IDTour)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    DateTime today = DateTime.Now;
                    PromotionCode Pcode = db.PromotionCodes.FirstOrDefault(c => c.Code == code
                    && today <= c.EndDay && today >= c.StartDay && c.Status == true);
                    if (Pcode != null)
                    {
                        /*
                         1-het ma
                         2-ok co ma va con luot dung
                         3-loi ko co ma nao
                         */
                        if (Pcode.Used >= Pcode.Total)
                        {
                            return Json(new { Result = "OK", Message = "1" });
                        }
                        else
                        {
                            return Json(new { Result = "OK", Message = "2", rate = Pcode.Rate / 100 });
                        }
                    }
                    return Json(new { Result = "ERROR", Message = "3" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        public ActionResult SubmitInvoidOnePay(string idOrder, double deposit)
        {
            PaymentConfigOnePay pay = new PaymentConfigOnePay();
            using (var db = new MyDbDataContext())
            {
                pay = db.PaymentConfigOnePays.FirstOrDefault();
            }
            if (pay != null)
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                VPCRequest conn = new VPCRequest("https://onepay.vn/vpcpay/vpcpay.op");

                conn.SetSecureSecret(pay.Hashcode);
                conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
                conn.AddDigitalOrderField("Title", "onepay paygate");
                conn.AddDigitalOrderField("vpc_Locale", "en");
                conn.AddDigitalOrderField("vpc_Version", "2");
                conn.AddDigitalOrderField("vpc_Command", "pay");
                conn.AddDigitalOrderField("vpc_Merchant", pay.MerchantId);
                conn.AddDigitalOrderField("vpc_AccessCode", pay.AccessCode);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", idOrder);
                conn.AddDigitalOrderField("vpc_OrderInfo", idOrder);
                conn.AddDigitalOrderField("vpc_Amount", deposit.ToString());
                conn.AddDigitalOrderField("vpc_ReturnURL", pay.WebSite + "/BookTour/MessageOnePay");
                conn.AddDigitalOrderField("vpc_TicketNo", ip);
                String url = conn.Create3PartyQueryString();
                return Redirect(url);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult MessageOnePay(string vpc_OrderInfo, string vpc_TxnResponseCode, string vpc_SecureHash)
        {
            PaymentConfigOnePay paymentConfig = new PaymentConfigOnePay();
            using (var db = new MyDbDataContext())
            {
                paymentConfig = db.PaymentConfigOnePays.FirstOrDefault();
                SendEmail sendEmail =
                      db.SendEmails.FirstOrDefault(
                          a => a.Type == TypeSendEmail.BookTour && a.LanguageID == Request.Cookies["LanguageID"].Value);

                int state = 0;
                string message = "";
                var conn = new VPCRequest("https://onepay.vn/vpcpay/vpcpost.op");
                conn.SetSecureSecret(paymentConfig.Hashcode);
                // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
                string hashvalidateResult = conn.Process3PartyResponse(Request.QueryString);
                // Lay gia tri tham so tra ve tu cong thanh toan
                var merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "Unknown");
                if (hashvalidateResult == "CORRECTED" && vpc_TxnResponseCode.Trim() == "0")
                {
                    message = "Transaction was paid successful";
                    state = 1;
                    ViewBag.Messages = sendEmail.Success;
                }
                else if (hashvalidateResult == "INVALIDATED" && vpc_TxnResponseCode.Trim() == "0")
                {
                    message = "Transaction is pending";
                    state = 2;
                    ViewBag.Messages = sendEmail.Error;
                }
                else
                {
                    message = "Transaction was not paid successful";
                    state = 3;
                    ViewBag.Messages = sendEmail.Error;
                }
                ViewBag.Message = message;
                ViewBag.State = state;
                if (state == 1)
                {
                    // do something
                    return View();
                }
                return View();
            }
        }

        //Add Review Clident
        [HttpPost]
        public ActionResult AddReview(Review rv, string Country, HttpPostedFileBase file)
        {
            ViewBag.Status = 0;
            // Xử lý file up lên
            try
            {
                if (file != null && file.ContentLength <= 300000)
                {
                    if (ImageExtensions.Contains(Path.GetExtension(file.FileName).ToUpperInvariant()))
                    {
                        var fileName = StringHelper.ConvertToAlias(rv.FullName) + '-' + DateTime.Now.ToString("yyyymmddMMss") + System.IO.Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Files/Images/AvataUser"), fileName);

                        file.SaveAs(path);
                        ///Xử lý dữ liệu
                        ///
                        rv.ProfileImages = "/Files/Images/AvataUser/" + fileName;
                        rv.TimeReview = DateTime.Now;
                        rv.DisplayStatus = false;
                        rv.Address = rv.Address + " - " + Country;
                        MyDbDataContext db = new MyDbDataContext();
                        db.Reviews.InsertOnSubmit(rv);
                        db.SubmitChanges();
                        ViewBag.name = rv.FullName;
                    }
                    else
                    {
                        ViewBag.notifi = "Unsupported file type has been uploaded";
                        ViewBag.Status = 2;
                    }
                }
                else
                {
                    ViewBag.notifi = "file too large!";
                    ViewBag.Status = 2;
                }
            }
            catch (Exception e)
            {
                ViewBag.Status = 1;
                ViewBag.description = e.Message;
            }
            return View();
        }

        public JsonResult LoadDataReview(int pageNumber, int menuID, int ID)
        {
            using (var db = new MyDbDataContext())
            {
                var lst = CommentController.GetListReview(menuID, ID);
                var data = lst.Select(a => new
                {
                    a.FullName,
                    a.ProfileImages,
                    TimeReview = a.TimeReview.ToString("dd/MM/yyy"),
                    a.KindOfTrip,
                    a.UseService,
                    rate = new int[a.Point],
                    unrate = new int[5 - a.Point],
                    a.Title,
                    a.Content,
                    a.ItineraryPoint,
                    a.FoodDrinkPoint,
                    a.AccomodationsPoint,
                    a.GuidePoint,
                    a.ActivityPoint
                }).Take(pageNumber).ToList();
                if (pageNumber > data.Count() + 5)
                {
                    return Json(new { status = false, data = data });
                }
                return Json(new { status = true, data = data });
            }
        }
    }
}