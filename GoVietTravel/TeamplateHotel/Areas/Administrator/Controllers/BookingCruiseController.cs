using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class BookingCruiseController : Controller
    {

        private readonly  MyDbDataContext db = new MyDbDataContext();
        //
        // GET: /Administrator/BookingCruise/
        public ActionResult Index()
        {
            ViewBag.Title = "Danh sách đặt Tàu";
            return View();
        }
        public ActionResult BookingCruiseDetail(int id)
        {
            ViewBag.Title = "Chi tiết đặt tàu";
            BookCruise bokcruise = db.BookCruises.FirstOrDefault(a => a.ID == id);
            return View(bokcruise);
        }
        [HttpPost]
        public JsonResult ListBookingCruise(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var list = db.BookCruises.OrderBy(a => a.CreateDate).Select(a => new
                {
                    a.ID,
                    a.FullName,
                    a.NameCruise,
                    a.Departure,
                    a.Email,
                    a.CreateDate
                }).OrderByDescending(a => a.CreateDate).ToList();
                return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
  

    }
}
