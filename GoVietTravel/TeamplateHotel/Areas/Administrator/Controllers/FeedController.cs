using System;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Security;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class FeedController : BaseController
    {
        public object ModulePermission { get; private set; }

        public ActionResult Index()
        { 
            ViewBag.Title = "feedback";
            return View();
        }
        [HttpPost]
        public JsonResult List(int jtStartIndex = 0,int role = 0,  int jtPageSize = 0, string jtSorting = null)
        {
            //End check
            try
            {
                var db = new MyDbDataContext();

                var list = db.FeedBacks.ToList();
                //Return result to jTable
                return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", message = ex.Message });
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(FeedBack model)
        {
            //End check
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var FeedBack = new FeedBack
                        {
                            Name = model.Name,
                            Content = model.Content,
                        };
                        db.FeedBacks.InsertOnSubmit(FeedBack);
                        db.SubmitChanges();
                        string message = "Insert successful";
                        return Json(new { Result = "OK", Message = message, Record = model });
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return Json(new { Result = " Error", Errors = ModelState.Errors(), Message = "Input data are not correct in form" }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(FeedBack model)
        {
            //End check
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var edit = db.FeedBacks.FirstOrDefault(b => b.ID == model.ID);
                        if (edit != null)
                        {
                            edit.Name = model.Name;
                            edit.Content = model.Content;
                           
                            db.SubmitChanges();

                            string message = "Update successful";
                            return Json(new { Result = "OK", Message = message });
                        }
                        else
                        {
                            return Json(new { Result = "ERROR", Message = "Video is not exist" });
                        }
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return Json(new { Result = " Error", Errors = ModelState.Errors(), Message = "Input data are not correct in form" }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var del = db.FeedBacks.FirstOrDefault(c => c.ID == Id);
                    if (del != null)
                    {
                        db.FeedBacks.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Delete successful" });
                    }
                    return Json(new { Result = "ERROR", Message ="Video is not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
            }
        }

    }
}
