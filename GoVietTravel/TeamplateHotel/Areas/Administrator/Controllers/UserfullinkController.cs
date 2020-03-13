using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Database;
using ProjectLibrary.Security;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class UserfullinkController : BaseController
    {
        // GET: /Administrator/UseFullLink/
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "UseFullLink";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
                List<UseFullLink> records = db.UseFullLinks.ToList();
               
                TempData["Messages"] = "Successful";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<EUserfullink> listAdv =
                        db.UseFullLinks.Select(a => new EUserfullink
                        {
                            ID = a.Id,
                            Name = a.Name,
                            Url = a.Url,
                            Image = a.Image,
                            Location = a.Location,
                            Type = a.Type,
                            MenuCountry = a.MenuCountry
                        }).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = listAdv, TotalRecordCount = listAdv.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(EUserfullink model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var adv = new UseFullLink
                        {
                            Name = model.Name,
                            Url = model.Url,
                            Image = model.Image,
                            Type = model.Type,
                            Location = model.Location,
                            MenuCountry = model.MenuCountry,
                        };
                        db.UseFullLinks.InsertOnSubmit(adv);
                        db.SubmitChanges();

                        string message = "Successful";
                        return Json(new { Result = "OK", Message = message, Record = model });
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "OK", Message = "Error: " + exception.Message });
                    }
                }

                return
                    Json(
                        new
                        {
                            Result = " OK",
                            Errors = ModelState.Errors(),
                            Message = "Error data"
                        }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(EUserfullink model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        UseFullLink UseFullLink = db.UseFullLinks.FirstOrDefault(b => b.Id == model.ID);
                        if (UseFullLink != null)
                        {
                            UseFullLink.Name = model.Name;
                            UseFullLink.Url = model.Url;
                            UseFullLink.Image = model.Image;
                            UseFullLink.Type = model.Type;
                            UseFullLink.Location = model.Location;
                            UseFullLink.MenuCountry = model.MenuCountry;

                            db.SubmitChanges();
                            string message = "Successful";
                           
                            return Json(new { Result = "OK", Message = message, Record = model });
                        }
                        else
                        {
                            return Json(new { Result = "ERROR", Message = "dose not exist" });
                        }
                    }
                }
                catch (Exception exception)
                {
                    return Json(new { Result = "OK", Message = "Error: " + exception.Message });
                }
            }
            return
                Json(
                    new
                    {
                        Result = " OK",
                        Errors = ModelState.Errors(),
                        Message = "error data"
                    }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    UseFullLink del = db.UseFullLinks.FirstOrDefault(c => c.Id == id);
                    if (del != null)
                    {
                        db.UseFullLinks.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Successful" });
                    }
                    return Json(new { Result = "ERROR", Message = "does not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }
}