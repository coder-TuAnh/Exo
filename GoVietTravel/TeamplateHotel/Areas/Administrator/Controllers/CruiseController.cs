//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Web.Mvc;
//using ProjectLibrary.Config;
//using ProjectLibrary.Database;
//using ProjectLibrary.Utility;
//using TeamplateHotel.Areas.Administrator.EntityModel;
//using System.Reflection;
//using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;

//namespace TeamplateHotel.Areas.Administrator.Controllers
//{
//    public class CruiseController : BaseController
//    {
//        MyDbDataContext db = new MyDbDataContext();
//        public ActionResult Index()
//        {
//            ViewBag.Title = "Danh sách tàu";
//            ViewData["MenuTitile"] = db.Menus.Select(x => x).Where(x => x.Type == SystemMenuType.Cruise).ToList();
//            return View();
//        }

//        #region cacMethodChinh

//        // lay danh sach tau
//        public JsonResult List(string name = "", string menuID = "0", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
//        {
//            try
//            {
//                /*
//                 if menuID 0 thi show tat ca
//                 nguoi lai show ra nhung tour khach hang yeu cau
//                 */
//                if (menuID == "0")
//                {
//                    var list = db.Cruises.Select(a => new
//                    {
//                        a.ID,
//                        a.Name,
//                        a.Price,
//                        a.Image,
//                        a.Location,
//                        a.Star,
//                        a.MenuID,
//                        a.UpdateDay
//                    }).
//                    Where(x => x.Name.Contains(name))
//                    .OrderByDescending(a => a.UpdateDay).ToList();
//                    return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count() });
//                }
//                else
//                {
//                    var list = db.Cruises.Select(a => new
//                    {
//                        a.ID,
//                        a.Name,
//                        a.Price,
//                        a.Image,
//                        a.Location,
//                        a.Star,
//                        a.MenuID,
//                    }).
//                        Where(x => x.Name.Contains(name) && x.MenuID.Contains(menuID))
//                        .OrderBy(a => a.Name).ToList();
//                    return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count() });
//                }

//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }

//        // Thêm va chinh tau
//        public ActionResult AddCruise(int id)
//        {
//            List<SelectListItem> listmenu = new List<SelectListItem>();
//            //listmenu.Add(new SelectListItem() { Value = "--Chọn Menu--", Text = "0" });
//            //db.Menus.Where(a => a.Type != SystemMenuType.Article && a.Type != SystemMenuType.Contact).ToList()
//            foreach (var b in db.Menus.Where(a => a.Type==SystemMenuType.Cruise).ToList())
//            {
//                listmenu.Add(new SelectListItem() { Value = b.ID.ToString(),Text =b.Title});
//            }
//            // lay danh sach cac tau
//            ViewBag.ListMenuID = new SelectList(listmenu,"Value","Text");
//            if (id == 0)
//            {
//                ViewBag.listDay = GetListDay();
//                ViewBag.cmd = "Create";
//                var cruise = new ECruise();

//                return View(cruise);
//            }
//            ViewBag.cmd = "Update";
//            Cruise model = db.Cruises.FirstOrDefault(a => a.ID == id);
//            if (model == null)
//            {
//                return PartialView("admin/Cruise/Index");
//            }
//            //ngay cap nhat bai viet
//            ViewBag.UpdateTime = model.UpdateDay;
//            ViewBag.listDay = GetListDay();
//            var ecruise = new ECruise
//            {
//                ID = model.ID,
//                Name = model.Name,
//                Alias = model.Alias,
//                Rate = model.Rate,
//                Action =  model.Action,
//                Location = model.Location,
//                Star = model.Star,
//                Price = model.Price,
//                PriceSale = model.PriceSale,
//                Duration = model.Duration,
//                Freeservic = model.Freeservic,
//                Departure = DateTime.Now.ToString("dd/MM/yyyy"),
//                //MenuID = model.MenuID,
//                UpdateDay = model.UpdateDay,
//                Image = model.Image,
//                Cruisegallery = model.Cruisegallery,
//                Description = model.Description,
//                BestCruise = model.BestCruise,
//                MetaDescription = model.MetaDescription,
//                MetaKeywords = model.MetaKeywords,
//                Home = model.Home,
//            };
//            if (!string.IsNullOrEmpty(model.MenuID))
//            {
//                string[] select = model.MenuID.Split(',');
//                ecruise.MenuID = select;
//            }
//            List<Cruisetab> cruisetab = db.Cruisetabs.Where(a => a.IDCruise == id).ToList();
//            ecruise.Cruitabs = cruisetab;
//            return View(ecruise);
//        }
//        [HttpPost]
//        [ValidateInput(false)]
//        public JsonResult Create(FormCollection frm)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var model = new Cruise
//                    {
//                        Name = frm["Name"],
//                        MetaDescription = frm["MetaDescription"],// 2truong de SEO trang web
//                        MetaKeywords = frm["MetaKeywords"],
//                        Alias = frm["Alias"],
//                        Rate = 0,
//                        Location = frm["Location"],
//                        Star = Int32.Parse(frm["Star"]),
//                        Action = frm["Action"],
//                        Price = Math.Round(float.Parse(frm["Price"]), 2),
//                        PriceSale = Math.Round(float.Parse(frm["PriceSale"]), 2),
//                        Duration = frm["Duration"],
//                        Departure = DateTime.Now,
//                        UpdateDay = DateTime.Now,
//                        MenuID = frm["MenuID"],
//                        Image = frm["Image"],
//                        Cruisegallery = "",
//                        Freeservic = frm["Freeservic"],
//                        Description = frm["Description"],
//                        About = frm["About"],
//                        Home = true,
//                    };
//                    var checkbestcruise = frm["BestCruise"];
//                    if (string.IsNullOrEmpty(checkbestcruise))
//                    {
//                        model.BestCruise = false;
//                    }
//                    else
//                    {
//                        model.BestCruise = true;
//                    }
//                    db.Cruises.InsertOnSubmit(model);
//                    db.SubmitChanges();
//                    int countconten = Int32.Parse(frm["countContenttab"]);
//                    for (int i = 0; i < countconten; i++)
//                    {
//                        var cruisetab = new Cruisetab
//                        {
//                            IDCruise = model.ID,
//                            Name = frm["TabTours[" + i + "].TitleTab"],
//                            Content = frm["TabTours[" + i + "].Content"],
//                            Price = float.Parse(frm["TabTours[" + i + "].Price"]),
//                        };
//                        db.Cruisetabs.InsertOnSubmit(cruisetab);
//                        db.SubmitChanges();
//                    }
//                    //double Cruisequality = double.Parse(frm["Cruisequality"]);
//                    //double FoodDrink = double.Parse(frm["FoodDrink"]);
//                    //double Cabinquality = double.Parse(frm["Cabinquality"]);
//                    //double Staffquality = double.Parse(frm["Staffquality"]);
//                    //double Entertainment = double.Parse(frm["Entertainment"]);
//                    //var total = (Cruisequality + FoodDrink + Cabinquality + Staffquality + Entertainment) / 5;
//                    return Json(new { success = true, id = model.ID });
//                }

//                catch (Exception ex)
//                {
//                    return Json(new { success = false });
//                }
//            }
//            return Json(new { success = false });

//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public JsonResult Update(FormCollection frm)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    int id = Int32.Parse(frm["ID"]);
//                    Cruise model = db.Cruises.FirstOrDefault(a => a.ID == id);
//                    if (model == null)
//                    {
//                        return Json(new { success = false });
//                    }
//                    model.Name = frm["Name"];
//                    model.MetaDescription = frm["MetaDescription"];
//                    model.MetaKeywords = frm["MetaKeywords"];
//                    model.Alias = frm["Alias"];
//                    model.Location = frm["Location"];
//                    model.Star = Int32.Parse(frm["Star"]);
//                    model.Action = frm["Action"];
//                    model.Price = Math.Round(float.Parse(frm["Price"]), 2);
//                    model.PriceSale = Math.Round(float.Parse(frm["PriceSale"]), 2);
//                    model.Duration = frm["Duration"];
//                    model.Departure = DateTime.Now;
//                    model.UpdateDay = DateTime.Now;
//                    model.MenuID = frm["MenuID"];
//                    model.Image = frm["Image"];
//                    model.Freeservic = frm["Freeservic"];
//                    model.Description = frm["Description"];
//                    model.About = frm["About"];
//                    var checkbestcruise = frm["BestCruise"];
//                    if (string.IsNullOrEmpty(checkbestcruise))
//                    {
//                        model.BestCruise = false;
//                    }
//                    else
//                    {
//                        model.BestCruise = true;
//                    }
//                    model.Home = true;
//                    db.SubmitChanges();
//                    var listcruiseold = db.Cruisetabs.Where(a => a.IDCruise == model.ID).ToList();

//                    var okpm = frm["countContenttab"];
//                    int? countconten = Int32.Parse(frm["countContenttab"] != "" ? frm["countContenttab"] : "0");
//                    for (int i = 0; i < countconten; i++)
//                    {
//                        if (frm["TabTours[" + i + "].id"] == "None")
//                        {
//                            var cruisetab = new Cruisetab
//                            {
//                                IDCruise = model.ID,
//                                Name = frm["TabTours[" + i + "].TitleTab"],
//                                Content = frm["TabTours[" + i + "].Content"],
//                                Price = float.Parse(frm["TabTours[" + i + "].Price"]),
//                            };
//                            db.Cruisetabs.InsertOnSubmit(cruisetab);
//                            db.SubmitChanges();
//                        }
//                        else
//                        {
//                            var tab = db.Cruisetabs.FirstOrDefault(a => a.ID.ToString() == frm["TabTours[" + i + "].id"]);
//                            tab.Name = frm["TabTours[" + i + "].TitleTab"];
//                            tab.Content = frm["TabTours[" + i + "].Content"];
//                            tab.Price = float.Parse(frm["TabTours[" + i + "].Price"]);
//                            db.SubmitChanges();
//                        }

//                    }
//                    //Delete Cruitab 

//                    foreach (var i in listcruiseold)
//                    {
//                        bool check = false;
//                        for (int a = 0; a < countconten; a++)
//                        {
//                            var element = frm["TabTours[" + a + "].id"];
//                            if (element == i.ID.ToString())
//                            {
//                                check = true;
//                            }
//                        }
//                        if (check == false)
//                        {
//                            db.Cruisetabs.DeleteOnSubmit(i);
//                            db.SubmitChanges();
//                        }
//                    }
//                    return Json(new { success = true, id = id });


//                }
//                catch (Exception ex)
//                {
//                    return Json(new { success = false });
//                }
//            }
//            return Json(new { success = false });

//        }

//        [HttpPost]
//        public JsonResult Delete(int id)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    Cruise del = db.Cruises.FirstOrDefault(c => c.ID == id);
//                    if (del != null)
//                    {
//                        //xóa hết hình ảnh của phòng này
//                        List<Cabin> listCabin = db.Cabins.Where(c => c.IDCruise == id).ToList();

//                        // XOA TAP cruise
//                        List<Cruisetab> listTab = db.Cruisetabs.Where(c => c.IDCruise == id).ToList();
//                        foreach (Cruisetab item in listTab)
//                        {
//                            db.Cruisetabs.DeleteOnSubmit(item);
//                        }
//                        //xóa hết cabin cau tau này
//                        foreach (Cabin cabin in listCabin)
//                        {
//                            db.Cabins.DeleteOnSubmit(cabin);
//                        }
//                        db.Cruises.DeleteOnSubmit(del);
//                        db.SubmitChanges();
//                        return Json(new { Result = "OK", Message = "Xóa thành công" });
//                    }
//                    return Json(new { Result = "ERROR", Message = "Bản Ghi không tồn tại" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }
//        // lay danh sach cabin
//        public JsonResult Listcabinofcruise(string idcruise)
//        {
//            try
//            {
//                var listcabin = db.Cabins.Where(a => a.IDCruise.ToString() == idcruise).OrderBy(a => a.Name).ToList();
//                var lst = listcabin.Select(a => new
//                {
//                    a.ID,
//                    a.IDCruise,
//                    a.Name,
//                    a.Price,
//                    a.Size,
//                    a.MaxAdults,
//                    a.Bed,
//                    a.Description,
//                    a.Content,
//                    a.Image,
//                    a.Cabingallery,
//                });
//                return Json(new { data = lst, success = true, JsonRequestBehavior.AllowGet });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, JsonRequestBehavior.AllowGet });
//            }
//        }
//        // lay danh sach anh cua cruise
//        public JsonResult Listgalleryfcruise(string idcruise)
//        {
//            try
//            {
//                var listcabin = db.Cruises.FirstOrDefault(a => a.ID.ToString() == idcruise);
//                return Json(new { data = listcabin.Cruisegallery, success = true, JsonRequestBehavior.AllowGet });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, JsonRequestBehavior.AllowGet });
//            }

//        }
//        #endregion


//        #region CacHamPhu
//        private SelectList GetListDay()
//        {
//            var list = new List<object>();
//            foreach (FieldInfo fieldInfo in typeof(Duration).GetFields())
//            {
//                if (fieldInfo.FieldType.Name != "Duration")
//                    continue;
//                var attribute = Attribute.GetCustomAttribute(fieldInfo,
//                    typeof(DisplayAttribute)) as DisplayAttribute;

//                list.Add(attribute != null
//                    ? new { name = fieldInfo.Name, display = attribute.Name }
//                    : new { name = fieldInfo.Name, display = fieldInfo.Name });
//            }

//            return new SelectList(list, "name", "display");
//        }
//        [HttpPost]
//        // them anh cho tau
//        public JsonResult AddgalleryPhoto(int id, string name)
//        {
//            Cruise model = db.Cruises.FirstOrDefault(a => a.ID == id);
//            if (model != null)
//            {
//                List<CruiseGallery> listDetail = new List<CruiseGallery>();

//                var images = new CruiseGallery
//                {
//                    NameImages = name
//                };
//                if (string.IsNullOrEmpty(model.Cruisegallery))
//                {

//                    listDetail.Add(images);
//                    var addgallery = JsonConvert.SerializeObject(listDetail);
//                    model.Cruisegallery = addgallery;
//                }
//                else
//                {
//                    listDetail = JsonConvert.DeserializeObject<List<CruiseGallery>>(model.Cruisegallery);
//                    listDetail.Add(images);
//                    model.Cruisegallery = JsonConvert.SerializeObject(listDetail);
//                }
//                db.SubmitChanges();

//            }
//            return Json(new { success = true });
//        }
//        [HttpPost]
//        // xoa anh cua tau
//        public JsonResult DeletePhoto(int id, string name)
//        {
//            var cruise = db.Cruises.FirstOrDefault(a => a.ID == id);
//            if (cruise != null)
//            {
//                List<CruiseGallery> listDetail = new List<CruiseGallery>();
//                listDetail = JsonConvert.DeserializeObject<List<CruiseGallery>>(cruise.Cruisegallery);
//                var model = listDetail.FirstOrDefault(a => a.NameImages == name);
//                listDetail.Remove(model);
//                cruise.Cruisegallery = JsonConvert.SerializeObject(listDetail);
//                db.SubmitChanges();
//            }
//            //var folder = Server.MapPath("~/Areas/Administrator/Content/FileImageCruise/ImgGallery/");
//            //var filePath = folder + name;
//            //System.IO.File.Delete(filePath);
//            return Json(new { success = true });
//        }
//        #endregion

//    }
//}
