//using ProjectLibrary.Database;
//using ProjectLibrary.Security;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace TeamplateHotel.Areas.Administrator.Controllers
//{
//    public class CustomizeTripController : Controller
//    {
//        //
//        // GET: /Administrator/CustomizeTrip/

//        public ActionResult Index()
//        {
//                ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
//                ViewBag.Title = "Trang tùy chỉnh chuyến đi";
//                return View();

//        }

//        [HttpPost]
//        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
//        {
//            try
//            {
//                var db = new MyDbDataContext();
//                List<CustomizeTrip> listContact = db.CustomizeTrips.ToList();

//                var records = listContact.Select(a => new
//                {
//                    a.ID,
//                    a.FullName,
//                    a.Phone,
//                    a.Email,
//                    a.Country,
//                    a.IPAdress,
//                    a.DepartureDate,
//                    a.TotalLength,
//                    a.HotelGrade,
//                    a.NumberOfClients,
//                    a.NeedVietnamVisa,
//                    a.VietNam,
//                    a.Lao,
//                    a.Cambodia,
//                    a.OtherRequest,
//                    a.Describe
//                }).Skip(jtStartIndex).Take(jtPageSize).ToList();
//                //Return result to jTable
//                return Json(new { Result = "OK", Records = records, TotalRecordCount = listContact.Count() });
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
//            CustomizeTrip del = db.CustomizeTrips.FirstOrDefault(a => a.ID == id);
//            if (del == null)
//            {
//                return Json(new { Result = "ERROR", Message = "Liên hệ không tồn tại" });
//            }
//            db.CustomizeTrips.DeleteOnSubmit(del);
//            db.SubmitChanges();
//            return Json(new { Result = "OK", Message = "Xóa liên hệ thành công" });
//        }

//        [HttpGet]
//        public ActionResult Detail(int Id)
//        {
//            var db = new MyDbDataContext();
//            CustomizeTrip detail = db.CustomizeTrips.FirstOrDefault(a => a.ID == Id);
//            if (detail == null)
//            {
//                TempData["Messages"] = "Liên hệ không tồn tại";
//                return RedirectToAction("Index");
//            }
//            return View("Detail", detail);
//        }
//    }

//}