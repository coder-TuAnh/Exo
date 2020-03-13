using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using ProjectLibrary.Database;
using System.IO;

namespace TeamplateHotel.Controllers
{
    public class FeedBackController : Controller
    {
        //
        // GET: /EmailMarketing/
        [HttpPost]
        public JsonResult SaveEmail(string Name, string Content )
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var checkEmail = db.FeedBacks.FirstOrDefault(a => a.Name == Name && a.Content == Content );
                    if (checkEmail != null)
                    {
                        return Json(new { result = "Error", message = "Email is exist" });
                    }
                    FeedBack marketing = new FeedBack
                    {
                       Name = Name,
                        Content = Content,
                    };
                    db.FeedBacks.InsertOnSubmit(marketing);
                    db.SubmitChanges();
                    return Json(new { result = "ok" });
                }
            }
            catch (Exception)
            {
                return Json(new {result = "Error", message="Error!"});
            }
        }

        
    }
}
