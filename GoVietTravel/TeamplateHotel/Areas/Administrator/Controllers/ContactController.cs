using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectLibrary.Database;
using ProjectLibrary.Security;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class ContactController : BaseController
    {
        //
        // GET: /Administrator/Contact/
        public ActionResult Index()
        {
            //var db = new MyDbDataContext();

            //string cookieClient = Request.Cookies["name_client"].Value;
            //string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
            //string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
            //var user = db.Users.FirstOrDefault(a => a.UserName == userName);
            //if (user.UserContent == true)
            //{
            //    int cout = 0;
            //    HttpCookie langCookie = Request.Cookies["lang_client"];
            //    while (langCookie != null)
            //    {
            //        langCookie.Expires = DateTime.Now.AddDays(-30);
            //        HttpContext.Response.Cookies.Add(langCookie);
            //        cout++;
            //        if (cout == 10)
            //            break;
            //    }
            //    cout = 0;
            //    HttpCookie nameCookie = Request.Cookies["name_client"];
            //    while (nameCookie != null)
            //    {
            //        nameCookie.Expires = DateTime.Now.AddDays(-30);
            //        HttpContext.Response.Cookies.Add(nameCookie);
            //        cout++;
            //        if (cout == 10)
            //            break;
            //    }
            //    CurrentSession.ClearAll();
            //    return Redirect("http://swallowtravel.com/admin");
            //}
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Contact";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var db = new MyDbDataContext();
                List<Contact> listContact = db.Contacts.ToList();

                var records = listContact.Select(a => new
                {
                    a.ID,
                    CreateDate = a.CreateDate.ToString(),
                    a.FullName,
                    a.Tel,
                    a.Email,
          
                }).Skip(jtStartIndex).Take(jtPageSize).ToList();
                //Return result to jTable
                return Json(new {Result = "OK", Records = records, TotalRecordCount = listContact.Count()});
            }
            catch (Exception ex)
            {
                return Json(new {Result = "ERROR", ex.Message});
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var db = new MyDbDataContext();
            Contact del = db.Contacts.FirstOrDefault(a => a.ID == id);
            if (del == null)
            {
                return Json(new {Result = "ERROR", Message = "does not exist"});
            }
            db.Contacts.DeleteOnSubmit(del);
            db.SubmitChanges();
            return Json(new {Result = "OK", Message = "Successful" });
        }

        [HttpGet]
        public ActionResult Detail(int Id)
        {
            var db = new MyDbDataContext();
            Contact detail = db.Contacts.FirstOrDefault(a => a.ID == Id);
            if (detail == null)
            {
                TempData["Messages"] = "does not exist";
                return RedirectToAction("Index");
            }
            return View("Detail", detail);
        }
    }
}