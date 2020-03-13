using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using TeamplateHotel.Areas.Administrator.EntityModel;
using TeamplateHotel.Areas.Administrator.Models;
using TeamplateHotel.Areas.Administrator.ModelShow;
using System.Globalization;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class SliderController : BaseController
    {
        // GET: /Administrator/Slider/
        public ActionResult Index()
        {
            LoadData();
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Sliders";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
                List<Slider> records =
                    db.Sliders.Where(r => r.LanguageID == Request.Cookies["lang_client"].Value).ToList();
                foreach (Slider record in records)
                {
                    string itemAdv = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemAdv, out index);
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
                using (var db = new MyDbDataContext())
                {
                    var records = db.Sliders.Select(a=> new {
                        a.ID,
                        a.Title,
                        a.Image,
                        a.Link,
                        a.ViewAll,
                        a.Status,
                        a.Index
                    }).ToList();
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "add slide";
            LoadData();
            ViewBag.Menus = LoadData("", Request.Cookies["lang_client"].Value);
            var model = new ESlider();
            model.ViewAll = true;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ESlider model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (model.ViewAll)
                        {
                            model.MenuIDs = "";
                        }
                        var slider = new Slider
                        {
                            LanguageID = Request.Cookies["lang_client"].Value,
                            Title = model.Title,
                            MenuIDs = model.MenuIDs,
                            //MenuID = model.MenuID,
                            Image = model.Image,
                            Link = model.Link,
                            Index = 0,
                            ViewAll = model.ViewAll,
                            Status = model.Status,
                            Description = model.Description,
                        };

                        db.Sliders.InsertOnSubmit(slider);
                        db.SubmitChanges();

                        TempData["Messages"] = "Successful";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        LoadData();
                        ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                LoadData();
                ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Slider detailSlider = db.Sliders.FirstOrDefault(a => a.ID == id);
                if (detailSlider == null)
                {
                    TempData["Messages"] = "Slide not exist";
                    return RedirectToAction("Index");
                }
                LoadData();
                var slider = new ESlider
                {
                    ID = detailSlider.ID,
                    Title = detailSlider.Title,
                    MenuIDs = detailSlider.MenuIDs,
                    //MenuID = detailSlider.MenuID,
                    Image = detailSlider.Image,
                    Link = detailSlider.Link,
                    ViewAll = (bool)detailSlider.ViewAll,
                    Status = (bool)detailSlider.Status,
                    Description = detailSlider.Description,
                };
                ViewBag.Title = "udpate slide";
                ViewBag.Menus = LoadData(slider.MenuIDs, Request.Cookies["lang_client"].Value);
                return View(slider);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ESlider model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        Slider slider = db.Sliders.FirstOrDefault(b => b.ID == model.ID);
                        if (model.ViewAll)
                        {
                            model.MenuIDs = "";
                        }

                        if (slider != null)
                        {
                            slider.Title = model.Title;
                            slider.Image = model.Image;
                            slider.Link = model.Link;
                            slider.Status = model.Status;
                            slider.ViewAll = model.ViewAll;
                            slider.MenuIDs = model.MenuIDs;
                            //slider.MenuID = model.MenuID;
                            slider.Description = model.Description;
                            slider.LanguageID = Request.Cookies["lang_client"].Value;
                            db.SubmitChanges();
                            TempData["Messages"] = "Successful";
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception exception)
                {
                    LoadData();
                    ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View(model);
                }
            }
            ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Slider del = db.Sliders.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Sliders.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Successful" });
                    }
                    return Json(new { Result = "ERROR", Message = "Slide not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        //Lấy danh sách menu khi thay đổi hotel
        public List<MenuCheck> LoadData(string menuIDs, string languageID)
        {
            //check logged
            var menuIsSelect = new List<MenuCheck>();
            List<Menu> listMenu =
                MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId, languageID);
            var listMenuIds = new int[1];
            if (string.IsNullOrEmpty(menuIDs) == false)
            {
                listMenuIds =
                    menuIDs.Substring(0, menuIDs.Length - 1)
                        .Split(',')
                        .Select(n => Convert.ToInt32(n))
                        .ToArray();
            }
            menuIsSelect =
                listMenu.Select(a => new MenuCheck
                {
                    Checked = listMenuIds.Contains(a.ID) ? "checked" : "",
                    Level = (int)a.Level,
                    ID = a.ID,
                    Title = a.Title
                }).ToList();
            return menuIsSelect;
        }


        public void LoadData()
        {
            var listMenu = new List<SelectListItem>();
            listMenu.Add(new SelectListItem { Value = "0", Text = "---Select a menu---" });
            List<Menu> getListMenu =
                MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId,
                    Request.Cookies["lang_client"].Value)
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