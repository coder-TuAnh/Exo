using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using TeamplateHotel.Areas.Administrator.EntityModel;
using TeamplateHotel.Areas.Administrator.ModelShow;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class MenuController : BaseController
    {
        [HttpGet]
        public ActionResult MainMenu()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.MenuLocation = GetLocaltion(SystemMenuLocation.MainMenu);
            return View("Index");
        }

        [HttpGet]
        public ActionResult SecondMenu()
        {
            if (TempData["Messages"] != null)
            {
                ViewBag.Messages = TempData["Messages"];
            }
            ViewBag.MenuLocation = GetLocaltion(SystemMenuLocation.SecondMenu);
            return View("Index");
        }

        [HttpGet]
        public ActionResult SecondMenuTop()
        {
            if (TempData["Messages"] != null)
            {
                ViewBag.Messages = TempData["Messages"];
            }
            ViewBag.MenuLocation = GetLocaltion(SystemMenuLocation.SecondMenuTop);
            return View("Index");
        }

        [HttpPost]
        public JsonResult List(int locationID = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                SystemMenuLocation locationMenu = GetLocaltion(locationID);
                List<Menu> listMenu = GetListMenu(locationMenu.LocationId, Request.Cookies["lang_client"].Value);
                //var test = from m in listMenu
                //           where m.ParentID==0
                //           select m;

                //var listMenuShow1 = from m in listMenu
                //                    join a in test on m.ParentID equals a.ID
                                   
                //                    select new ShowMenu
                //                    {
                //                        Index = m.Index,
                //                        ID = m.ID,
                //                        Status = (bool)m.Status,
                //                        Title = m.Title,
                //                        //TypeMenu = m.,
                //                        Level = (int)m.Level
                //                    };

                List<ShowMenu> listMenuShow =
                    listMenu.Where(m => m.Location == locationMenu.LocationId)
                        .Join(SystemMenuType.CategoryType, a => a.Type, b => b.Key, (a, b) => new ShowMenu
                        {
                            Index = a.Index,
                            ID = a.ID,
                            Status = (bool)a.Status,
                            Title = a.Title,
                            TypeMenu = b.Value,
                            Level = (int)a.Level
                        }).ToList();
                return Json(new {Result = "OK", Records = listMenuShow, TotalRecordCount = listMenuShow.ToList().Count});
            }
            catch (Exception ex)
            {
                return Json(new {Result = "ERROR", message = ex.Message});
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ListData();
            SystemMenuLocation systemMenuLocation = GetLocaltion(0);
            ViewBag.MenuLocation = systemMenuLocation;
            return View();
        }

        [HttpPost]
        public ActionResult Create(EMenu model)
        {
            if (ModelState.IsValid)
            {
                SystemMenuLocation menuLocation = GetLocaltion(model.Location);
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        if (string.IsNullOrEmpty(model.Alias))
                        {
                            model.Alias = StringHelper.ConvertToAlias(model.Title);
                        }
                        //Kiểm tra xem alias thuộc hotel này đã tồn tại chưa
                        Menu checkMenuAlias =
                            db.Menus.FirstOrDefault(m => m.Alias == model.Alias);
                        if (checkMenuAlias != null)
                        {
                            ModelState.AddModelError("Alias", "Menu not exist");
                        }

                        if (model.Type == SystemMenuType.OutSite)
                        {
                            if (string.IsNullOrEmpty(model.Link))
                            {
                                ModelState.AddModelError("Link", "Please enter link");
                            }
                        }

                        Menu menuParent = db.Menus.FirstOrDefault(c => c.ID == model.ParentID);
                        if (menuParent != null)
                            model.Level = (int)model.ParentID > 0 ? (int)menuParent.Level + 1 : 0;
                        else
                        {
                            model.Level = 0;
                        }
                        if (model.Type == SystemMenuType.Home)
                        {
                            model.Alias = "";
                        }

                        var menu = new Menu
                        {
                            Alias = model.Alias,
                            LanguageID = Request.Cookies["lang_client"].Value,
                            Index = 0,
                            Location = menuLocation.LocationId,
                            Level = model.Level,
                            MetaDescription = string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.MetaDescription,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle,
                            ParentID = model.ParentID,
                            Status = model.Status,
                            Title = model.Title,
                            Type = model.Type,
                            Link = model.Link,
                            Description = model.Description,
                            Image = model.Image,
                            Showhome = model.Showhome,
                        };
                        db.Menus.InsertOnSubmit(menu);
                        db.SubmitChanges();
                        TempData["Messages"] = "Successful";
                        return RedirectToAction(menuLocation.AliasMenu);
                    }

                }
                catch (Exception exception)
                {
                    ListData();
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View();
                }
            }
            ListData();
            return View();
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Menu menu = db.Menus.FirstOrDefault(m => m.ID == id);

                SystemMenuLocation menuLocation = GetLocaltion(0);
                if (menu != null)
                {
                    var model = new EMenu
                    {
                        Alias = menu.Alias,
                        Index = menu.Index,
                        Level = (int)menu.Level,
                        Location = (int)menu.Location,
                        ID = menu.ID,
                        MetaDescription = menu.MetaDescription,
                        MetaTitle = menu.MetaTitle,
                        ParentID = menu.ParentID,
                        Status = (bool)menu.Status,
                        Title = menu.Title,
                        Type = (int)menu.Type,
                        Link = menu.Link,
                        Description = menu.Description,
                        Image = menu.Image,
                        Showhome= menu.Showhome ?? false,
                    };
                    ListData();
                    ViewBag.MenuLocation = GetLocaltion(0);
                    return View(model);
                }
                TempData["Messages"] = "Menu not exist";
                return RedirectToAction(menuLocation.AliasMenu);
            }
        }

        [HttpPost]
        public ActionResult Update(EMenu model)
        {
            //Kiểm tra xem alias thuộc hotel này đã tồn tại chưa
            var db = new MyDbDataContext();
            Menu checkMenuAlias = db.Menus.FirstOrDefault(m => m.Alias == model.Alias && m.ID != model.ID);
            if (checkMenuAlias != null)
            {
                ModelState.AddModelError("Alias", "Menu is exist");
            }
            //nếu không hiển thị trên tất cả khách sạn thì menu bắt buộc phải thuộc một khách sạn

            if (model.Type == SystemMenuType.OutSite)
            {
                if (string.IsNullOrEmpty(model.Link))
                {
                    ModelState.AddModelError("Link", "Please enter link");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Menu edit = db.Menus.FirstOrDefault(m => m.ID == model.ID);

                    if (edit != null)
                    {
                        model.Alias = !string.IsNullOrEmpty(model.Alias) ? model.Alias : StringHelper.ConvertToAlias(model.Title);
                        SystemMenuLocation menuLocation = GetLocaltion((int)edit.Location);
                        Menu firstOrDefault = db.Menus.FirstOrDefault(c => c.ID == model.ParentID);
                        if (firstOrDefault != null)
                            edit.Level = model.ParentID > 0 ? firstOrDefault.Level + 1 : 0;
                        else
                        {
                            edit.Level = 0;
                        }
                        if (model.Type == SystemMenuType.Home)
                        {
                            model.Alias = "";
                        }

                        edit.Alias =  model.Alias;
                        edit.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle;
                        edit.Title = model.Title;
                        edit.ParentID = model.ParentID;
                        edit.Type = model.Type;
                        edit.Status = model.Status;
                        edit.MetaDescription = string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.MetaDescription;
                        edit.Link = model.Link;
                        edit.Image = model.Image;
                        edit.Description = model.Description;
                        edit.Showhome = model.Showhome;
                        db.SubmitChanges();

                        //Cập nhật lại Level menu con
                        List<Menu> menuChilds = db.Menus.Where(a => a.ParentID == edit.ID).ToList();
                        foreach (var menuChild in menuChilds)
                        {
                            menuChild.Level = edit.Level + 1;
                            db.SubmitChanges();
                        }

                        TempData["Messages"] = "Successful";
                        return RedirectToAction(menuLocation.AliasMenu);
                    }
                    SystemMenuLocation menulocation2 = GetLocaltion(0);
                    TempData["Messages"] = "Menu not exist";
                    return RedirectToAction(menulocation2.AliasMenu);
                }
                catch (Exception exception)
                {
                    ListData();
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View(model);
                }
            }
            ListData();
            ViewBag.Messages = "Error data";
            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Menu del = db.Menus.FirstOrDefault(c => c.ID == id);
                    //kiểm tra xem thằng này có menu con không
                    List<Menu> listMenu = db.Menus.Where(m => m.ParentID == del.ID).ToList();
                    if (del != null)
                    {
                        db.Menus.DeleteAllOnSubmit(listMenu);
                        db.SubmitChanges();
                        db.Menus.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new {Result = "OK", Message = "Successful" });
                    }
                    return Json(new {Result = "OK", Message = "Menu not exist"});
                }
            }
            catch (Exception ex)
            {
                return Json(new {Result = "ERROR", ex.Message});
            }
        }

        [HttpPost]
        public ActionResult UpdateIndex(int locationID)
        {
            SystemMenuLocation localtionMenu = GetLocaltion(locationID);
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Menu> menus =
                        db.Menus.Where(c => c.LanguageID == Request.Cookies["lang_client"].Value).ToList();

                    foreach (Menu item in menus)
                    {
                        string requestIndex = Request.Params[string.Format("Sort[{0}].Index", item.ID)];
                        int index;
                        int.TryParse(requestIndex, out index);
                        Menu temp = db.Menus.FirstOrDefault(c => c.ID == item.ID);
                        if (temp != null)
                        {
                            temp.Index = index;
                            db.SubmitChanges();
                        }
                    }
                }
                TempData["Messages"] = "successful";
                return RedirectToAction(localtionMenu.AliasMenu, "Menu");
            }
            catch (Exception ex)
            {
                TempData["Messages"] = "Error: " + ex.Message;
                return RedirectToAction(localtionMenu.AliasMenu, "Menu");
            }
        }

        public SystemMenuLocation GetLocaltion(int locationId)
        {
            SystemMenuLocation menuLocation =
                SystemMenuLocation.ListLocationMenu().ToList().FirstOrDefault(m => m.LocationId == locationId);
            if (menuLocation != null)
            {
                return menuLocation;
            }
            string aliasMenu = Request.QueryString["location"];
            if (string.IsNullOrEmpty(aliasMenu) == false)
            {
                return SystemMenuLocation.ListLocationMenu().ToList().FirstOrDefault(m => m.AliasMenu == aliasMenu);
            }
            return new SystemMenuLocation {AliasMenu = "MainMenu", TitleMenu = "Main menu", LocationId = 1};
        }

        //lấy danh sách menu theo ngôn ngữ, theo hotel, theo vị trí, theo AllHotel
        public static List<Menu> GetListMenu(int locationId, string languageID)
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> listMenuRoot = db.Menus.ToList();
                listMenuRoot = listMenuRoot.Where(m => m.ParentID == 0).OrderBy(m => m.Index).ToList();

                listMenuRoot = locationId == 0
                    ? listMenuRoot
                    : listMenuRoot.Where(a => a.Location == locationId).ToList();

                List<Menu> listMenu = listMenuRoot;
                Menu menuMaxLevel = locationId == 0
                    ? db.Menus.OrderByDescending(m => m.Level).FirstOrDefault()
                    : db.Menus.Where(m => m.Location == locationId).OrderByDescending(m => m.Level).FirstOrDefault();

                int level = 0;
                if (menuMaxLevel != null)
                {
                    level = (int)menuMaxLevel.Level;
                }

                if (level > 0)
                {
                    for (int i = 1; i <= level; i++)
                    {
                        var listMenuTemp = new List<Menu>();
                        List<Menu> listMenuByLevel;

                        listMenuByLevel =
                            db.Menus.Where(m => m.Level == i).ToList();
                        listMenuByLevel = locationId == 0
                            ? listMenuByLevel
                            : listMenuByLevel.Where(a => a.Location == locationId).ToList();

                        foreach (Menu menu in listMenu)
                        {
                            listMenuTemp.Add(menu);
                            listMenuTemp.AddRange(listMenuByLevel.Where(m => m.ParentID == menu.ID).ToList());
                        }
                        listMenu = listMenuTemp;
                    }
                }
                else
                {
                    listMenu = listMenuRoot;
                }
                return listMenu;
            }
        }

        //Lấy danh sách typemenu, parentMene
        public void ListData()
        {
            var listTypeMenu = new List<SelectListItem>();
            listTypeMenu.Add(new SelectListItem
            {
                Text = "Select type show",
                Value = "0",
            });
           
           
            listTypeMenu.AddRange(SystemMenuType.CategoryType.Select(a => new SelectListItem
            {
                Text = a.Value,
                Value = a.Key.ToString()
            }).ToList());
            ViewBag.ListTypeMenu = listTypeMenu;

            SystemMenuLocation menuLocation = GetLocaltion(0);

            List<Menu> listParentMenu = GetListMenu(menuLocation.LocationId, Request.Cookies["lang_client"].Value);
            foreach (Menu menu in listParentMenu)
            {
                string treeNode = "";
                if (menu.Level > 0)
                {
                    for (int i = 1; i <= menu.Level; i++)
                    {
                        treeNode += "|--";
                    }
                }
                menu.Title = treeNode + menu.Title;
            }
            var selectListMenu = new List<SelectListItem>();

            selectListMenu.Add(new SelectListItem
            {
                Text = "Not in any category",
                Value = "0"
            });
            selectListMenu.AddRange(listParentMenu.Select(a => new SelectListItem
            {
                Text = a.Title,
                Value = a.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList());
            ViewBag.ListParentMenu = selectListMenu;
        }
    }
}