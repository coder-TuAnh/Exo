//using ProjectLibrary.Database;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using TeamplateHotel.Areas.Administrator.EntityModel;

//namespace TeamplateHotel.Areas.Administrator.Controllers
//{
//    public class ManageVisaController : Controller
//    {
//        //
//        // GET: /Administrator/ManageVisa/
//        #region BookVisa

//        public ActionResult Index()
//        {
//            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
//            ViewBag.Title = "Quản lý order Visa";
//            return View();
//        }

//        [HttpPost]
//        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
//        {
//            try
//            {
//                var db = new MyDbDataContext();
//                List<BookVisa> list = db.BookVisas.ToList();

//                var records = list.Select(a => new
//                {
//                    a.IDBook,
//                    a.DateOfArrival,
//                    a.ArrivalAirport,
//                    a.ProcessingTime,
//                    a.FullName,
//                    a.Gender,
//                    a.Email,
//                    a.Country,
//                    a.City,
//                    a.PhoneNumber,
//                    a.SocialMedia,
//                    a.SpecialRequests,
//                    a.TotalPrice,
//                    a.IDTypeOfVisa
//                }).Skip(jtStartIndex).Take(jtPageSize).ToList();
//                //Return result to jTable
//                return Json(new { Result = "OK", Records = records, TotalRecordCount = list.Count() });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }

//        [HttpPost]
//        public JsonResult Delete(int id)
//        {
//            var db = new MyDbDataContext();
//            List<DetailBookVisa> lstdt = db.DetailBookVisas.Where(c => c.BookVisaID == id).ToList();
//            foreach (DetailBookVisa dt in lstdt)
//            {
//                db.DetailBookVisas.DeleteOnSubmit(dt);
//                db.SubmitChanges();
//            }
//            BookVisa del = db.BookVisas.FirstOrDefault(a => a.IDBook == id);
//            if (del == null)
//            {
//                return Json(new { Result = "ERROR", Message = "không tồn tại" });
//            }
//            db.BookVisas.DeleteOnSubmit(del);
//            db.SubmitChanges();
//            return Json(new { Result = "OK", Message = "Xóa thành công" });
//        }

//        [HttpGet]
//        public ActionResult Detail(int Id)
//        {
//            var db = new MyDbDataContext();
//            BookVisa detail = db.BookVisas.SingleOrDefault(a => a.IDBook == Id);
//            List<DetailBookVisa> lstDetailBookVisa = db.DetailBookVisas.Where(a => a.BookVisaID == Id).ToList();
//            if (detail == null)
//            {
//                TempData["Messages"] = "không tồn tại";
//                return RedirectToAction("Index");
//            }
//            ViewBag.BookVisaDetail = detail;
//            ViewBag.lstDetailBookVisa = lstDetailBookVisa;
//            return View();
//        }

//        #endregion
//        #region Type Visa
//        public ActionResult TypeVisa()
//        {
//            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
//            ViewBag.Title = "Quản lý loại visa";
//            return View();
//        }
//        public JsonResult ListTypeVisa(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    List<TypeVisa> listTypeVisa = db.TypeVisas.ToList();
//                    var records = listTypeVisa.Select(a => new
//                    {
//                        a.ID,
//                        a.Name,
//                        a.Price
//                    }).Skip(jtStartIndex).Take(jtPageSize).ToList();
//                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listTypeVisa.Count() });
//                }

//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", message = ex.Message });
//            }
//        }

//        [HttpPost]
//        public ActionResult CreateTypeVisa(ETypeVisa model)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                if (model.Name != "" && model.Price != null)
//                {
//                    try
//                    {
//                        var insert = new TypeVisa
//                        {
//                            Name = model.Name,
//                            Price = model.Price
//                        };

//                        db.TypeVisas.InsertOnSubmit(insert);
//                        db.SubmitChanges();
//                        model.ID = insert.ID;

//                        return Json(new { Result = "OK", Message = "Thêm loại visa thành công", Record = model });
//                    }
//                    catch (Exception exception)
//                    {
//                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
//                    }
//                }
//                return
//                    Json(
//                        new
//                        {
//                            Result = "Error",
//                            Errors = ModelState.Errors(),
//                            Message = "Dữ liệu đầu vào không đúng định dạng"
//                        });
//            }
//        }

//        [HttpPost]
//        public ActionResult EditTypeVisa(ETypeVisa model)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                if (model.Name != "" && model.Price != null)
//                {
//                    TypeVisa edit = db.TypeVisas.FirstOrDefault(b => b.ID == model.ID);
//                    if (edit == null)
//                    {
//                        TempData["Messages"] = "không tồn tại trong hệ thống";
//                        return RedirectToAction("Index");
//                    }

//                    try
//                    {
//                        edit.Name = model.Name;
//                        edit.Price = model.Price;
//                        db.SubmitChanges();

//                        return Json(new { Result = "OK", Message = "Edit successful" });
//                    }
//                    catch (Exception exception)
//                    {
//                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
//                    }
//                }
//                return
//                    Json(
//                        new
//                        {
//                            Result = "Error",
//                            Errors = ModelState.Errors(),
//                            Message = "Dữ liệu đầu vào không đúng định dạng"
//                        });
//            }
//        }

//        [HttpPost]
//        public JsonResult DeleteTypeVisa(int id)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    TypeVisa del = db.TypeVisas.FirstOrDefault(c => c.ID == id);
//                    if (del != null)
//                    {
//                        List<BookVisa> lstb = db.BookVisas.Where(c => c.IDTypeOfVisa == id).ToList();
//                        foreach (BookVisa b in lstb)
//                        {
//                            List<DetailBookVisa> lstdt = db.DetailBookVisas.Where(c => c.BookVisaID == b.IDBook).ToList();
//                            foreach (DetailBookVisa dt in lstdt)
//                            {
//                                db.DetailBookVisas.DeleteOnSubmit(dt);
//                                db.SubmitChanges();
//                            }
//                            db.BookVisas.DeleteOnSubmit(b);
//                            db.SubmitChanges();
//                        }
//                        db.TypeVisas.DeleteOnSubmit(del);
//                        //Xóa
//                        db.SubmitChanges();
//                        return Json(new { Result = "OK", Message = "Xóa thành công" });
//                    }
//                    return Json(new { Result = "ERROR", Message = "Không tồn tại trong hệ thống" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
//            }
//        }
//        #endregion

//        #region Type Visa
//        public ActionResult ProcessingTimeVisa()
//        {
//            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
//            ViewBag.Title = "Quản lý thời gian xử lý Order";
//            return View();
//        }
//        public JsonResult ListProcessingTimeVisa(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    List<ProcessingTimeVisa> listProcessingTimeVisa = db.ProcessingTimeVisas.ToList();
//                    var records = listProcessingTimeVisa.Select(a => new
//                    {
//                        a.ID,
//                        a.Name,
//                        a.Price
//                    }).Skip(jtStartIndex).Take(jtPageSize).ToList();
//                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listProcessingTimeVisa.Count() });
//                }

//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", message = ex.Message });
//            }
//        }

//        [HttpPost]
//        public ActionResult CreateProcessingTimeVisa(EProcessingTimeVisa model)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                if (model.Name != "")
//                {
//                    try
//                    {
//                        var insert = new ProcessingTimeVisa
//                        {
//                            Name = model.Name,
//                            Price = model.Price
//                        };

//                        db.ProcessingTimeVisas.InsertOnSubmit(insert);
//                        db.SubmitChanges();
//                        model.ID = insert.ID;

//                        return Json(new { Result = "OK", Message = "Thêm loại visa thành công", Record = model });
//                    }
//                    catch (Exception exception)
//                    {
//                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
//                    }
//                }
//                return
//                    Json(
//                        new
//                        {
//                            Result = "Error",
//                            Errors = ModelState.Errors(),
//                            Message = "Dữ liệu đầu vào không đúng định dạng"
//                        });
//            }
//        }

//        [HttpPost]
//        public ActionResult EditProcessingTimeVisa(EProcessingTimeVisa model)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                if (model.Name != "")
//                {
//                    ProcessingTimeVisa edit = db.ProcessingTimeVisas.FirstOrDefault(b => b.ID == model.ID);
//                    if (edit == null)
//                    {
//                        TempData["Messages"] = "không tồn tại trong hệ thống";
//                        return RedirectToAction("Index");
//                    }

//                    try
//                    {
//                        edit.Name = model.Name;
//                        edit.Price = model.Price;
//                        db.SubmitChanges();

//                        return Json(new { Result = "OK", Message = "Edit successful" });
//                    }
//                    catch (Exception exception)
//                    {
//                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
//                    }
//                }
//                return
//                    Json(
//                        new
//                        {
//                            Result = "Error",
//                            Errors = ModelState.Errors(),
//                            Message = "Dữ liệu đầu vào không đúng định dạng"
//                        });
//            }
//        }

//        [HttpPost]
//        public JsonResult DeleteProcessingTimeVisa(int id)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    ProcessingTimeVisa del = db.ProcessingTimeVisas.FirstOrDefault(c => c.ID == id);
//                    if (del != null)
//                    {
//                        List<BookVisa> lstb = db.BookVisas.Where(c => c.ProcessingTime == id).ToList();
//                        foreach (BookVisa b in lstb)
//                        {
//                            List<DetailBookVisa> lstdt = db.DetailBookVisas.Where(c => c.BookVisaID == b.IDBook).ToList();
//                            foreach (DetailBookVisa dt in lstdt)
//                            {
//                                db.DetailBookVisas.DeleteOnSubmit(dt);
//                                db.SubmitChanges();
//                            }
//                            db.BookVisas.DeleteOnSubmit(b);
//                            db.SubmitChanges();
//                        }
//                        db.ProcessingTimeVisas.DeleteOnSubmit(del);
//                        //Xóa
//                        db.SubmitChanges();
//                        return Json(new { Result = "OK", Message = "Xóa thành công" });
//                    }
//                    return Json(new { Result = "ERROR", Message = "Không tồn tại trong hệ thống" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
//            }
//        }
//        #endregion

//    }
//}