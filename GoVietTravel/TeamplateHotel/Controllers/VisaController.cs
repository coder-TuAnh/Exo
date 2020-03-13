//using Newtonsoft.Json;
//using ProjectLibrary.Database;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Web;
//using System.Web.Mvc;

//namespace TeamplateHotel.Controllers
//{
//    public class VisaController : Controller
//    {
//        //
//        // GET: /Visa/

//        public ActionResult Index()
//        {
//            return View();
//        }
//        public JsonResult GetTypeOfVisa()
//        {
//            try
//            {
//                var db = new MyDbDataContext();
//                List<TypeVisa> lst = db.TypeVisas.ToList();
//                List<TypeVisa> kq = new List<TypeVisa>();
//                foreach (TypeVisa t in lst)
//                {
//                    TypeVisa typeVisa = new TypeVisa();
//                    typeVisa.ID = t.ID;
//                    typeVisa.Name = t.Name;
//                    typeVisa.Price = t.Price;
//                    kq.Add(typeVisa);
//                }
//                return Json(kq, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception e)
//            {
//                return Json(e.Message, JsonRequestBehavior.AllowGet);
//            }

//        }
//        public JsonResult GetProcessingTime()
//        {
//            var db = new MyDbDataContext();
//            var lst = db.ProcessingTimeVisas.ToList();
//            return Json(lst, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Order(BookVisa b, List<DetailBookVisa> lst)
//        {
//            try
//            {
//                MyDbDataContext db = new MyDbDataContext();
//                if (CheckBookVisaInput(b))
//                {
//                    db.BookVisas.InsertOnSubmit(b);
//                    db.SubmitChanges();
//                    int BookVisaID = b.IDBook;
//                    foreach (DetailBookVisa dt in lst)
//                    {
//                        dt.BookVisaID = BookVisaID;
//                        db.DetailBookVisas.InsertOnSubmit(dt);
//                    }
//                    db.SubmitChanges();
//                    ViewBag.mail = b.Email;
//                    ViewBag.name = b.FullName;
//                }
//                else
//                {
//                    return RedirectToAction("index", "Visa");
//                }
//            }
//            catch
//            {
//                return RedirectToAction("index", "Visa");
//            }
//            return View();
//        }
//        public bool CheckBookVisaInput(BookVisa b)
//        {
//            if (b.FullName.Length > 50)
//            {
//                return false;
//            }

//            try
//            {
//                MailAddress m = new MailAddress(b.Email);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }

//        }
//    }
//}