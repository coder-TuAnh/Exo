using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using ProjectLibrary.Database;


namespace TeamplateHotel.Controllers
{
    public class EmailMarketingController : Controller
    {
        //
        // GET: /EmailMarketing/
        [HttpPost]
        public JsonResult SaveEmail(string emailMarketing)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var checkEmail = db.EmailMarketings.FirstOrDefault(a => a.Email == emailMarketing);
                    if (checkEmail != null)
                    {
                        return Json(new { result = "Error", message = "Email is exist" });
                    }
                    EmailMarketing marketing = new EmailMarketing
                    {
                        Email = emailMarketing,
                        Tel = "",
                        Note = "",
                    };
                    db.EmailMarketings.InsertOnSubmit(marketing);
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
