//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Web.Mvc;
//using ProjectLibrary.Config;
//using ProjectLibrary.Database;
//using ProjectLibrary.Utility;
//using TeamplateHotel.Areas.Administrator.EntityModel;

//namespace TeamplateHotel.Areas.Administrator.Controllers
//{
//    public class ReviewController : BaseController
//    {
//        // GET: /Administrator/Review/
//        public ActionResult Index()
//        {
//            LoadData();
//            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
//            ViewBag.Title = "Review";
//            return View();
//        }

//        [HttpPost]
//        public ActionResult UpdateIndex()
//        {
//            using (var db = new MyDbDataContext())
//            {
//                TempData["Messages"] = "Không có chức năng này";
//                return RedirectToAction("Index");
//            }
//        }

//        [HttpPost]
//        public JsonResult List(int menuId = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, int Pending = 0)
//        {
//            try
//            {
//                var db = new MyDbDataContext();
//                if (menuId == 0)
//                {
//                    var listRv = (from p in db.Reviews
//                                  join e in db.Menus
//                                  on p.MenuID equals e.ID
//                                  where e.LanguageID == Request.Cookies["lang_client"].Value
//                                  select p).ToList();
//                    if (Pending == 1)
//                    {
//                        listRv = listRv.Where(x => x.DisplayStatus == false).ToList();
//                    }
//                    else if (Pending == 2)
//                    {
//                        listRv = listRv.Where(x => x.DisplayStatus == true).ToList();
//                    }
//                    var records = listRv.Select(a => new
//                    {
//                        a.ID,
//                        a.Title,
//                        a.FullName,
//                        a.DisplayStatus,
//                        a.Point,
//                        a.UseService,
//                        titleMenu = db.Menus.SingleOrDefault(x => x.ID == a.MenuID).Title,
//                    }).Skip(jtStartIndex).Take(jtPageSize).OrderByDescending(a => a.ID).ToList();
//                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listRv.Count });
//                }
//                else
//                {
//                    var listRv = (from p in db.Reviews
//                                  join e in db.Menus
//                                  on p.MenuID equals e.ID
//                                  where p.MenuID == menuId && e.LanguageID == Request.Cookies["lang_client"].Value
//                                  select p).ToList();
//                    if (Pending == 1)
//                    {
//                        listRv = listRv.Where(x => x.DisplayStatus == false).ToList();
//                    }
//                    else if (Pending == 2)
//                    {
//                        listRv = listRv.Where(x => x.DisplayStatus == true).ToList();
//                    }
//                    var records = listRv.Select(a => new
//                    {
//                        a.ID,
//                        a.Title,
//                        a.FullName,
//                        a.DisplayStatus,
//                        a.Point,
//                        a.UseService,
//                        titleMenu = db.Menus.SingleOrDefault(x => x.ID == menuId).Title,
//                    }).Skip(jtStartIndex).Take(jtPageSize).OrderByDescending(a => a.ID).ToList();
//                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listRv.Count });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }

//        [HttpGet]
//        public ActionResult Create()
//        {
//            ViewBag.Title = "Add New Review";
//            LoadData();
//            var Review = new EReview();
//            return View(Review);
//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult Create(EReview model)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                if (ModelState.IsValid)
//                {
//                    try
//                    {
//                        var rv = new Review
//                        {
//                            ObjID = model.ObjID,
//                            Index = 0,
//                            MenuID = model.MenuID,
//                            FullName = model.FullName,
//                            Gender = model.Gender,
//                            Title = model.Title,
//                            Content = model.Content,
//                            Point = model.Point,
//                            TimeReview = model.TimeReview,
//                            Address = model.Address,
//                            DisplayStatus = model.DisplayStatus,
//                            UseService = model.UseService,
//                            Email = model.Email,
//                            KindOfTrip = model.KindOfTrip,
//                            ProfileImages = string.IsNullOrEmpty(model.ProfileImages) ? model.ProfileImages : model.ProfileImages,
//                            ItineraryPoint = model.ItineraryPoint,
//                            FoodDrinkPoint = model.FoodDrinkPoint,
//                            GuidePoint = model.GuidePoint,
//                            ActivityPoint = model.ActivityPoint,
//                            AccomodationsPoint = model.AccomodationsPoint,
//                        };

//                        db.Reviews.InsertOnSubmit(rv);
//                        db.SubmitChanges();

//                        TempData["Messages"] = "Add new Review success";
//                        return RedirectToAction("Index");
//                    }
//                    catch (Exception exception)
//                    {
//                        LoadData();
//                        ViewBag.Messages = "Error: " + exception.Message;
//                        return View(model);
//                    }
//                }
//                LoadData();
//                return View(model);
//            }
//        }

//        [HttpGet]
//        public ActionResult Update(int id)
//        {
//            ViewBag.Title = "Update Review";
//            var db = new MyDbDataContext();
//            Review detailReview = db.Reviews.FirstOrDefault(a => a.ID == id);
//            if (detailReview == null)
//            {
//                TempData["Messages"] = "Review does not exist";
//                return RedirectToAction("Index");
//            }
//            LoadData();
//            ViewBag.ObjID = detailReview.ObjID;
//            var Review = new EReview
//            {
//                ObjID = detailReview.ObjID,
//                Index = 0,
//                MenuID = detailReview.MenuID,
//                FullName = detailReview.FullName,
//                Gender = detailReview.Gender,
//                Title = detailReview.Title,
//                Content = detailReview.Content,
//                Point = detailReview.Point,
//                TimeReview = detailReview.TimeReview,
//                Address = detailReview.Address,
//                DisplayStatus = detailReview.DisplayStatus,
//                UseService = detailReview.UseService,
//                Email = detailReview.Email,
//                KindOfTrip = detailReview.KindOfTrip,
//                ProfileImages = detailReview.ProfileImages,
//                ItineraryPoint = detailReview.ItineraryPoint,
//                FoodDrinkPoint = detailReview.FoodDrinkPoint,
//                GuidePoint = detailReview.GuidePoint,
//                ActivityPoint = detailReview.ActivityPoint,
//                AccomodationsPoint = detailReview.AccomodationsPoint,
//            };
//            return View(Review);
//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult Update(EReview model)
//        {
//            var db = new MyDbDataContext();
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    Review rv = db.Reviews.FirstOrDefault(b => b.ID == model.ID);
//                    if (rv != null)
//                    {
//                        rv.ObjID = model.ObjID;
//                        rv.Index = model.Index;
//                        rv.MenuID = model.MenuID;
//                        rv.FullName = model.FullName;
//                        rv.Gender = model.Gender;
//                        rv.Title = model.Title;
//                        rv.Content = model.Content;
//                        rv.Point = model.Point;
//                        rv.TimeReview = model.TimeReview;
//                        rv.Address = model.Address;
//                        rv.DisplayStatus = model.DisplayStatus;
//                        rv.UseService = model.UseService;
//                        rv.Email = model.Email;
//                        rv.KindOfTrip = model.KindOfTrip;
//                        rv.ProfileImages = model.ProfileImages;
//                        rv.ItineraryPoint = model.ItineraryPoint;
//                        rv.FoodDrinkPoint = model.FoodDrinkPoint;
//                        rv.GuidePoint = model.GuidePoint;
//                        rv.ActivityPoint = model.ActivityPoint;
//                        rv.AccomodationsPoint = model.AccomodationsPoint;

//                        db.SubmitChanges();

//                        TempData["Messages"] = "The Review was successful";
//                        return RedirectToAction("Index");
//                    }
//                }
//                catch (Exception exception)
//                {
//                    LoadData();
//                    ViewBag.Messages = "Error: " + exception.Message;
//                    return View();
//                }
//            }
//            LoadData();
//            return View(model);
//        }

//        [HttpPost]
//        public JsonResult Delete(int id)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    Review del = db.Reviews.FirstOrDefault(c => c.ID == id);
//                    if (del != null)
//                    {
//                        db.Reviews.DeleteOnSubmit(del);
//                        db.SubmitChanges();
//                        return Json(new { Result = "OK", Message = "Clear Review success" });
//                    }
//                    return Json(new { Result = "ERROR", Message = "Review này không tồn tại" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }

//        public void LoadData()
//        {
//            var listMenu = new List<SelectListItem>();
//            listMenu.Add(new SelectListItem { Value = "0", Text = "---Select a menu---" });
//            List<Menu> getListMenu =
//                MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId,
//                    Request.Cookies["lang_client"].Value).
//                    Where(x => x.Type == SystemMenuType.Tour)
//                    .ToList();

//            foreach (Menu menu in getListMenu)
//            {
//                string subTitle = "";
//                for (int i = 1; i <= menu.Level; i++)
//                {
//                    subTitle += "|--";
//                }
//                menu.Title = subTitle + menu.Title;
//            }
//            listMenu.AddRange(getListMenu.Select(a => new SelectListItem
//            {
//                Value =
//                    a.ID.ToString(
//                        CultureInfo.InvariantCulture),
//                Text = a.Title
//            }).ToList());
//            ViewBag.ListMenu = listMenu;

//            //Get listLanguage
//            using (var db = new MyDbDataContext())
//            {
//                var listLanguage = new List<SelectListItem>();
//                List<Language> list = db.Languages.ToList();

//                listLanguage.AddRange(list.Select(a => new SelectListItem
//                {
//                    Value =
//                    a.ID.ToString(
//                        CultureInfo.InvariantCulture),
//                    Text = a.Name
//                }).ToList());

//                ViewBag.lstLanguage = listLanguage;
//            }
//            //init List Point
//            var listPoint = new List<SelectListItem>();
//            List<int> lstP = new List<int>();
//            for (int i = 1; i <= 5; i++)
//            {
//                lstP.Add(i);
//            }
//            listPoint.AddRange(lstP.Select(a => new SelectListItem
//            {
//                Value =
//                a.ToString(
//                    CultureInfo.InvariantCulture),
//                Text = a.ToString(
//                    CultureInfo.InvariantCulture)
//            }).ToList());

//            ViewBag.lstPoint = listPoint;
//            //init List KindOfTrip
//            var listKindOfTrip = new List<SelectListItem>();
//            List<string> lstkot = new List<string>();
//            lstkot.Add("Enterprise"); lstkot.Add("Couple");
//            lstkot.Add("Family"); lstkot.Add("Friend"); lstkot.Add("Alone");
//            listKindOfTrip.AddRange(lstkot.Select(a => new SelectListItem
//            {
//                Value =
//                a.ToString(
//                    CultureInfo.InvariantCulture),
//                Text = a.ToString(
//                    CultureInfo.InvariantCulture)
//            }).ToList());

//            ViewBag.lstKindOfTrip = listKindOfTrip;
//        }

//        public JsonResult ListObj(int menuId = 0)
//        {
//            try
//            {
//                int type;
//                var db = new MyDbDataContext();
//                if (menuId != 0)
//                {
//                    type = db.Menus.SingleOrDefault(x => x.ID == menuId).Type;
//                    switch (type)
//                    {
//                        case SystemMenuType.Tour:
//                            var listTour = db.Tours.Join(
//                         db.Menus.Where(m => m.ID == menuId && m.LanguageID == Request.Cookies["lang_client"].Value),
//                         a => a.MenuID, b => b.ID, (a, b) => new { a, b }).ToList();
//                            var recordsTour = listTour.Select(a => new
//                            {
//                                a.a.ID,
//                                a.a.Title,
//                            }).ToList();
//                            return Json(recordsTour, JsonRequestBehavior.AllowGet);

//                            //add key type
//                    }
//                }
//                else
//                {
//                    return Json(new { ID = "", Title = "Menu does not exist Object" }, JsonRequestBehavior.AllowGet);
//                }
//                return Json(new { ID = "", Title = "Please Select Menu" }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }

//        public JsonResult GetObj(int ObjID = 0, int MenuID = 0)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                int type;
//                if (MenuID != 0 && ObjID != 0)
//                {
//                    type = db.Menus.SingleOrDefault(x => x.ID == MenuID).Type;
//                    switch (type)
//                    {
//                        case SystemMenuType.Tour:
//                            var tour = db.Tours.FirstOrDefault(x => x.ID == ObjID && x.MenuID == MenuID);
//                            return Json(new { ID = tour.ID, Name = tour.Title, MenuID = tour.MenuID }, JsonRequestBehavior.AllowGet);

//                    }
//                }
//                return Json(new { ID = "Null", Name = "Null", MenuID = "Null" }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        public JsonResult CheckUseService(string Email, int menuId)
//        {
//            if (menuId != 0 && Email != "")
//            {
//                int type;
//                string email = Email.Trim().ToLower();
//                using (var db = new MyDbDataContext())
//                {
//                    type = db.Menus.SingleOrDefault(x => x.ID == menuId).Type;
//                    bool check = false;
//                    switch (type)
//                    {
//                        case SystemMenuType.Tour:
//                            var booktour = db.BookTours.FirstOrDefault(x => x.Email.Trim().ToLower() == email);
//                            if (booktour != null)
//                            {
//                                check = true;
//                            }
//                            break;
//                            //add key type
//                    }
//                    return Json(new { valid = check }, JsonRequestBehavior.AllowGet);
//                }
//            }
//            return Json(new { valid = false }, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult Approved(int id = 0,bool val=true)
//        {
//            var db = new MyDbDataContext();
//            try
//            {
//                Review rv = db.Reviews.FirstOrDefault(b => b.ID == id);
//                if (rv != null)
//                {
//                    rv.DisplayStatus = !val;
//                    db.SubmitChanges();
//                    return Json(new { Result = "The Review status update was successful" }, JsonRequestBehavior.AllowGet);
//                }else
//                {
//                    return Json(new { Result = "Không tồn tại" }, JsonRequestBehavior.AllowGet);
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR "+ex.Message });
//            }

//        }
//    }
//}