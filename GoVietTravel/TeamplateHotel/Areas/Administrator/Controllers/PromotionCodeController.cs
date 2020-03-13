using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectLibrary.Database;
using System.Threading;
using TeamplateHotel.Handler;
using TeamplateHotel.Areas.Administrator.EntityModel;
using ProjectLibrary.Security;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class PromotionCodeController : Controller
    {
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
            //    return View();
            //}
            LoadData();
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Promotion";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
                List<Tour> records = db.Tours.ToList();
                foreach (Tour record in records)
                {
                    string itemTour = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemTour, out index);
                    record.Index = index;
                    db.SubmitChanges();
                }
                TempData["Messages"] = "Successful";
                return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
            }
        }

        [HttpPost]
        public JsonResult List(int menuId = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var db = new MyDbDataContext();
                var listmenu = db.Menus.Where(m => m.LanguageID == Request.Cookies["lang_client"].Value).ToList();
                var listPro = new List<PromotionCode>();
                if (menuId == 0)
                {
                    listPro = db.PromotionCodes.ToList();
                }
                else
                {
                    listPro = db.PromotionCodes.ToList();
                }

                var records = listPro.Select(a => new
                {
                    a.ID,
                    a.Code,
                    a.StartDay,
                    a.EndDay,
                    a.Total,
                    a.Status,
                    a.Rate,
                    a.Used,
                }).Skip(jtStartIndex).Take(jtPageSize).OrderByDescending(a => a.ID).ToList();
                //Return result to jTable
                return Json(new { Result = "OK", Records = records, TotalRecordCount = listPro.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var db = new MyDbDataContext();

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
            //    return View();
            //}
            ViewBag.Title = "Add Promotion";
            LoadData();
            var Pro = new EPromotionCode();
            return View(Pro);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EPromotionCode model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (db.PromotionCodes.ToList().Find(x => x.Code == model.Code) != null)
                        {
                            TempData["Messages"] = "is exist code";
                            return View(model);
                        }
                        var pro = new PromotionCode
                        {
                            Code = model.Code,
                            Rate = model.Rate,
                            StartDay = model.StartDay,
                            EndDay = model.EndDay,
                            Total = model.Total,
                            Used = model.Used,
                            Description = model.Description,
                            Status = model.Status,
                        };

                        db.PromotionCodes.InsertOnSubmit(pro);
                        db.SubmitChanges();

                        TempData["Messages"] = "Successful";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        LoadData();
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                LoadData();
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var db = new MyDbDataContext();

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
            //    return View();
            //}

            ViewBag.Title = "Udpate Promotion";
       
            PromotionCode detailPro = db.PromotionCodes.FirstOrDefault(a => a.ID == id);
            if (detailPro == null)
            {
                TempData["Messages"] = "Does not exist";
                return RedirectToAction("Index");
            }
            LoadData();
            var pro = new EPromotionCode
            {
                Code = detailPro.Code,
                Rate = detailPro.Rate,
                StartDay = detailPro.StartDay,
                EndDay = detailPro.EndDay,
                Status = detailPro.Status,
                Total = detailPro.Total,
                Description = detailPro.Description,
                Used = detailPro.Used,
            };
            return View(pro);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EPromotionCode model)
        {
            //Kiểm tra xem alias thuộc tour này đã tồn tại chưa
            var db = new MyDbDataContext();

            if (ModelState.IsValid)
            {
                try
                {
                    PromotionCode Pro = db.PromotionCodes.FirstOrDefault(b => b.ID == model.ID);
                    if (Pro != null)
                    {
                        Pro.Code = model.Code;
                        Pro.StartDay = model.StartDay;
                        Pro.EndDay = model.EndDay;
                        Pro.Rate = model.Rate;
                        Pro.Status = model.Status;
                        Pro.Used = model.Used;
                        Pro.Total = model.Total;
                        Pro.Description = model.Description;

                        db.SubmitChanges();

                        TempData["Messages"] = "Successful";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception exception)
                {
                    LoadData();
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View();
                }
            }
            LoadData();
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    PromotionCode del = db.PromotionCodes.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.PromotionCodes.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Successful" });
                    }
                    return Json(new { Result = "ERROR", Message = "Does not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public void LoadData()
        {
            var ListTour = new List<SelectListItem>();
            ListTour.Add(new SelectListItem { Value = "0", Text = "---Select a Tour---" });
            List<Tour> data = new List<Tour>();
            using (var db = new MyDbDataContext())
            {
                data = db.Tours.ToList();
            }

            ListTour.AddRange(data.Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.Title
            }).ToList());
            ViewBag.ListTour = ListTour;
        }

        [HttpGet]
        public JsonResult GenerateCode()
        {
            string _code = "CODE";
            List<string> listkey = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                string s;
                s = CodeHelper.RandomString(4);
                Thread.Sleep(50);
                _code += "-" + s;
            }
            return Json(new { Result = "OK", code = _code }, JsonRequestBehavior.AllowGet);
        }
    }
}