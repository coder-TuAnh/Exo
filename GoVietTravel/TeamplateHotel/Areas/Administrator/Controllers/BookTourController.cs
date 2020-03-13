using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectLibrary.Database;
using ProjectLibrary.Security;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class BookTourController : BaseController
    {
        //
        // GET: /Administrator/BookTour/
        public ActionResult Index()
        {
            //using (var db = new MyDbDataContext())
            //{
            //    string cookieClient = Request.Cookies["name_client"].Value;
            //    string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
            //    string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
            //    var user = db.Users.FirstOrDefault(a => a.UserName == userName);
            //    if (user.UserContent == true)
            //    {
            //        int cout = 0;
            //        HttpCookie langCookie = Request.Cookies["lang_client"];
            //        while (langCookie != null)
            //        {
            //            langCookie.Expires = DateTime.Now.AddDays(-30);
            //            HttpContext.Response.Cookies.Add(langCookie);
            //            cout++;
            //            if (cout == 10)
            //                break;
            //        }
            //        cout = 0;
            //        HttpCookie nameCookie = Request.Cookies["name_client"];
            //        while (nameCookie != null)
            //        {
            //            nameCookie.Expires = DateTime.Now.AddDays(-30);
            //            HttpContext.Response.Cookies.Add(nameCookie);
            //            cout++;
            //            if (cout == 10)
            //                break;
            //        }
            //        CurrentSession.ClearAll();
            //        return Redirect("http://swallowtravel.com/admin");
            //    }
            //}
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Book tour";

            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var db = new MyDbDataContext();
                var records = db.BookTours.Select(a => new
                {
                    a.ID,
                    a.Code,
                    a.Gender,
                    a.FullName,
                    a.Departure,
                    a.Country,
                    a.CreateDate,
                    a.Total,
                    Tour = a.InfoBooking,
                    a.Email,
                }).OrderByDescending(a => a.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();

               
                var bookTour = from b in db.BookTours
                               join t in db.Tours on b.TourId equals t.ID
                               select new
                               {
                                   b.ID,
                                   b.Code,
                                   b.Gender,
                                   b.FullName,
                                   b.Departure,
                                   b.Country,
                                   b.CreateDate,
                                   b.Total,
                                   t.Title,
                                   
                                   //Tour = a.InfoBooking,
                                   b.Email,
                               };
                bookTour.OrderByDescending(a => a.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();
                //Return result to jTable
                return Json(new { Result = "OK", Records = bookTour, TotalRecordCount = db.BookTours.Count() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var db = new MyDbDataContext();

            BookTour del = db.BookTours.FirstOrDefault(a => a.ID == id);
            if (del == null)
            {
                return Json(new { Result = "ERROR", Message = "does not exist" });
            }
            db.BookTours.DeleteOnSubmit(del);
            db.SubmitChanges();
            return Json(new { Result = "OK", Message = "Successful" });
        }

        [HttpGet]
        public ActionResult Detail(int Id)
        {
            var db = new MyDbDataContext();

            string cookieClient = Request.Cookies["name_client"].Value;
            string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
            string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
            var user = db.Users.FirstOrDefault(a => a.UserName == userName);
            if (user.UserContent == true)
            {
                int cout = 0;
                HttpCookie langCookie = Request.Cookies["lang_client"];
                while (langCookie != null)
                {
                    langCookie.Expires = DateTime.Now.AddDays(-30);
                    HttpContext.Response.Cookies.Add(langCookie);
                    cout++;
                    if (cout == 10)
                        break;
                }
                cout = 0;
                HttpCookie nameCookie = Request.Cookies["name_client"];
                while (nameCookie != null)
                {
                    nameCookie.Expires = DateTime.Now.AddDays(-30);
                    HttpContext.Response.Cookies.Add(nameCookie);
                    cout++;
                    if (cout == 10)
                        break;
                }
                CurrentSession.ClearAll();
                return Redirect("http://swallowtravel.com/admin");
            }

            BookTour detail = db.BookTours.FirstOrDefault(a => a.ID == Id);
            if (detail == null)
            {
                TempData["Messages"] = "does not exist";
                return RedirectToAction("Index");
            }
            return View("Detail", detail);
        }
    }
}