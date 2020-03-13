using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class TourController : BaseController
    {
        // GET: /Administrator/Tour/
        public ActionResult Index()
        {
            LoadData();
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Page Tours";
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
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult List(int menuId = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var db = new MyDbDataContext();
                var listmenu = db.Menus.ToList();
                var listTour = new List<Tour>();
                if (menuId == 0)
                {
                    listTour = db.Tours.ToList();
                }
                else
                {
                    listTour = db.Tours.Where(a => a.MenuID == menuId).ToList();
                }

                var records = listTour.Join(db.Menus, a => a.MenuID, b => b.ID,
                            (a, b) => new
                            {
                                a.ID,
                                a.Title,
                                a.Index,
                                a.Home,
                                a.Deal,
                                a.Sale,
                                //a.a.StartDeal
                            }).Skip(jtStartIndex).Take(jtPageSize).OrderBy(a => a.Index).ToList();
                //Return result to jTable
                return Json(new { Result = "OK", Records = records, TotalRecordCount = listTour.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "add tour";
            LoadData();
            var tour = new ETour();
            return View(tour);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ETour model)
        {
            using (var db = new MyDbDataContext())
            {
                //Kiểm tra xem đã chọn đến chuyên mục con cuối cùng chưa
                //if (db.Menus.Any(a => a.ParentID == model.MenuID))
                //{
                //    ModelState.AddModelError("MenuId", "Phải chọn đến chuyên mục tour con cuối cùng");
                //}

                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Alias))
                    {
                        model.Alias = StringHelper.ConvertToAlias(model.Title);
                    }
                    try
                    {
                        DateTime? _start_deal;
                        int? _time_deal;
                        if (model.Deal)
                        {
                            _start_deal = model.StartDeal;
                            _time_deal = model.TimeDeal;
                        }
                        else
                        {
                            _start_deal = null;
                            _time_deal = null;
                        }
                        var tour = new Tour
                        {
                            MenuID = model.MenuID,
                            Title = model.Title,
                            Alias = model.Alias,
                            ToDes = model.ToDes,
                            FromDes = model.FromDes,
                            HotelService = model.HotelService,
                            ThreeStar = model.ThreeStar,
                            FourStar = model.FourStar,
                            FiveStar = model.FiveStar,
                            Deal = model.Deal,
                            StartDeal = _start_deal,
                            CheckCruise=model.CheckCruise,
                            TimeDeal = _time_deal,
                            Image = model.Image,
                            Deposit = model.Deposit,
                            Home=model.Home,
                            Index = 0,
                            Description = model.Description,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.MetaDescription,
                            Status = model.Status,
                            Price = model.Price,
                            PriceSale = model.PriceSale,
                            Hot = model.Hot,
                            Sale = model.Sale,
                            Address=model.Address,

                            //StartDeal = model.StartDeal,
                            TourOther = model.TourOther,
                        };

                        db.Tours.InsertOnSubmit(tour);
                        db.SubmitChanges();

                        //Thêm hình ảnh cho tour
                        if (model.EGalleryITems != null)
                        {
                            foreach (EGalleryITem itemGallery in model.EGalleryITems)
                            {
                                var gallery = new TourGallery
                                {
                                    LargeImage = itemGallery.Image,
                                    SmallImage = ReturnSmallImage.GetImageSmall(itemGallery.Image),
                                    TourID = tour.ID,
                                    ImageLage= itemGallery.Image,
                                };
                                db.TourGalleries.InsertOnSubmit(gallery);
                            }
                            db.SubmitChanges();
                        }
                        //Thêm tabtour
                        if (model.TabTours != null)
                        {
                            foreach (TabTour itemTabTour in model.TabTours)
                            {
                                var tabTour = new TabTour
                                {
                                    TourID = tour.ID,
                                    TitleTab = itemTabTour.TitleTab,
                                    Content = itemTabTour.Content,
                                    Price = itemTabTour.Price,
                                };

                                db.TabTours.InsertOnSubmit(tabTour);
                            }
                            db.SubmitChanges();
                        }

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
            ViewBag.Title = "udpate tour";
            var db = new MyDbDataContext();
            Tour detailTour = db.Tours.FirstOrDefault(a => a.ID == id);
            if (detailTour == null)
            {
                TempData["Messages"] = "Tour not exist";
                return RedirectToAction("Index");
            }
            LoadData();
            var tour = new ETour

            {

                ID = detailTour.ID,
                MenuID = detailTour.MenuID,
                Title = detailTour.Title,
                Alias = detailTour.Alias,
                ToDes = detailTour.ToDes,
                FromDes = detailTour.FromDes,
                //HotelService = (bool)detailTour.HotelService,
                //ThreeStar = (decimal)detailTour.ThreeStar,
                //FourStar = (decimal)detailTour.FourStar,
                //FiveStar = (decimal)detailTour.FiveStar,
                Deal = (bool)detailTour.Deal,
                ////StartDeal = detailTour.StartDeal,
                //TimeDeal = detailTour.TimeDeal,
                Image = detailTour.Image,
                Description = detailTour.Description,
                MetaTitle = detailTour.MetaTitle,
                MetaDescription = detailTour.MetaDescription,
                Status = (bool)detailTour.Home,
                Home = (bool)detailTour.Home,
                CheckCruise = (bool)detailTour.CheckCruise,
                //HighlightTour = (bool)detailTour.HighlightTour,
                
               

            ////Hot = detailTour.Hot,
            //Sale = (bool)detailTour.Sale,
            Price = (decimal)detailTour.Price,
                PriceSale = (decimal)detailTour.PriceSale,
                Address = detailTour.Address,
                //StartDeal = (bool)detailTour.StartDeal,
                //TourOther = (bool)detailTour.TourOther,
            };
            if (detailTour.Deposit == null)
            {
                tour.Deposit = 0;
            }
            else
            {
                tour.Deposit = (int)detailTour.Deposit;
            }
            //lấy danh sách hình ảnh
            //tour.EGalleryITems = db.TourGalleries.Where(a => a.TourID == detailTour.ID).Select(a => new EGalleryITem
            //{
            //    Image = a.ImageLage
            //}).ToList();
            var gallery = from a in db.TourGalleries
                                 where a.TourID == detailTour.ID
                                 select new EGalleryITem
                                 {
                                     Image = a.ImageLage
                                 };
            tour.EGalleryITems = gallery.ToList();

            //lấy danh sách tabtour
            List < TabTour > tabTours = db.TabTours.Where(a => a.TourID == detailTour.ID).ToList();
            tour.TabTours = tabTours;

            return View(tour);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ETour model)
        {
            //Kiểm tra xem alias thuộc tour này đã tồn tại chưa
            var db = new MyDbDataContext();

            //Kiểm tra xem đã chọn đến chuyên mục con cuối cùng chưa
            //var check = db.Menus.Where(a => a.ParentID == model.ID).ToList();
            //if (db.Menus.Any(a => a.ParentID == model.ID))
            //{
            //    ModelState.AddModelError("MenuId", "Vui lòng chọn đến chuyên mục tour con cuối cùng");
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    Tour tour = db.Tours.FirstOrDefault(b => b.ID == model.ID);
                    if (tour != null)
                    {
                        tour.MenuID = model.MenuID;
                        tour.Title = model.Title;
                        tour.Alias = model.Alias;
                        tour.ToDes = model.ToDes;
                        tour.FromDes = model.FromDes;
                        tour.HotelService = (bool)model.HotelService;
                        tour.ThreeStar = (decimal)model.ThreeStar;
                        tour.FourStar = (decimal)model.FourStar;
                        tour.FiveStar = (decimal)model.FiveStar;
                        tour.Deal = model.Deal;
                        tour.Image = model.Image;
                        tour.Description = model.Description;
                        tour.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle;
                        tour.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                            ? model.Title
                            : model.MetaDescription;
                        tour.Status = model.Status;
                        tour.Home = model.Home;
                        tour.Hot = model.Hot;
                        tour.Sale = model.Sale;
                        tour.Price = model.Price;
                        tour.PriceSale = model.PriceSale;
                        tour.Deposit = model.Deposit;
                        tour.Address = model.Address;

                        if (model.Deal == true)
                        {
                            tour.StartDeal = model.StartDeal;
                            tour.TimeDeal = model.TimeDeal;
                        }
                        else
                        {
                            tour.StartDeal = null;
                            tour.TimeDeal = null;
                        }
                        //tour.StartDeal = model.StartDeal;
                        tour.TourOther = model.TourOther;
                        db.SubmitChanges();

                        //xóa gallery cho tour
                        db.TourGalleries.DeleteAllOnSubmit(db.TourGalleries.Where(a => a.TourID == tour.ID).ToList());
                        //Thêm hình ảnh cho tour
                        if (model.EGalleryITems != null)
                        {
                            foreach (EGalleryITem itemGallery in model.EGalleryITems)
                            {
                                var gallery = new TourGallery
                                {
                                    ImageLage = itemGallery.Image,
                                    LargeImage=itemGallery.Image,
                                    SmallImage = ReturnSmallImage.GetImageSmall(itemGallery.Image),
                                    TourID = tour.ID,
                                };
                                db.TourGalleries.InsertOnSubmit(gallery);
                            }
                            db.SubmitChanges();
                        }
                        //xóa danh sách tabtour
                        db.TabTours.DeleteAllOnSubmit(db.TabTours.Where(a => a.TourID == tour.ID).ToList());

                        //Thêm tabtour
                        if (model.TabTours != null)
                        {
                            foreach (TabTour itemTabTour in model.TabTours)
                            {
                                var tabTour = new TabTour
                                {
                                    TourID = tour.ID,
                                    TitleTab = itemTabTour.TitleTab,
                                    Content = itemTabTour.Content,
                                    Price = itemTabTour.Price
                                };

                                db.TabTours.InsertOnSubmit(tabTour);
                            }
                            db.SubmitChanges();
                        }

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
                    Tour del = db.Tours.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        //xóa hết hình ảnh của tour này
                        db.TourGalleries.DeleteAllOnSubmit(db.TourGalleries.Where(a => a.TourID == del.ID).ToList());
                        //xóa hết tabtour của tour này
                        var tabtour = from a in db.TabTours
                                      where a.TourID == del.ID
                                      select a;
                      
                        db.TabTours.DeleteAllOnSubmit(tabtour);
                        db.Tours.DeleteOnSubmit(del);
                        //db.TabTours.DeleteAllOnSubmit(db.TabTours.Where(a => a.TourID == del.ID).ToList());

                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Successful" });
                    }
                    return Json(new { Result = "ERROR", Message = "Tour not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public void LoadData()
        {
            var listMenu = new List<SelectListItem>();
            listMenu.Add(new SelectListItem { Value = "0", Text = "---Select a menu---" });
            List<Menu> getListMenu =
                MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId,
                    Request.Cookies["lang_client"].Value)
                    .Where(a => a.Type == SystemMenuType.Tour)
                    .ToList();

            foreach (Menu menu in getListMenu)
            {
                string subTitle = "";
                for (int i = 1; i <= menu.Level; i++)
                {
                    subTitle += "|--";
                }
                menu.Title = subTitle + menu.Title;
            }
            listMenu.AddRange(getListMenu.Select(a => new SelectListItem
            {
                Value =
                    a.ID.ToString(
                        CultureInfo.InvariantCulture),
                Text = a.Title
            }).ToList());
            ViewBag.ListMenu = listMenu;
        }
    }
}