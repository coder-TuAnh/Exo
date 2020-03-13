using Newtonsoft.Json;
using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class CabinController : Controller
    {
        //
        // GET: /Administrator/Cabin/
        private readonly MyDbDataContext db = new MyDbDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult DeleteCabin(int id)
        {
            try
            {
                Cabin model = db.Cabins.FirstOrDefault(a => a.ID == id);
                db.Cabins.DeleteOnSubmit(model);
                db.SubmitChanges();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }

        }
        public ActionResult AddCabin(int id, int idcruise)
        {
            if (id == 0)
            {
                var ecabin = new ECabin();
                var listtab = new List<EPricetabCabin>();
                ViewBag.IsUpdate = false;
                ViewBag.IDCruise = idcruise;
                var listtabcruise = db.Cruisetabs.Where(a => a.IDCruise == idcruise && a.Price != 0).ToList();
                foreach (var i in listtabcruise)
                {
                    var tab = new EPricetabCabin
                    {
                        IDTabCruise = i.ID,
                        NameTabCruise = i.Name,
                        Price = 0,
                        Pricechildren = 0,
                        Pricesale = 0
                    };
                    listtab.Add(tab);
                }
                ecabin.listcruitab = listtab;
                return View(ecabin);
            }
            else
            {
                ViewBag.IsUpdate = true;
                ViewBag.IDCruise = idcruise;
                Cabin model = db.Cabins.FirstOrDefault(a => a.ID == id);
                if (model == null)
                {
                    return PartialView("admin/Cruise/Index");
                }
                var ecabin = new ECabin
                {
                    ID = model.ID,
                    Name = model.Name,
                    Pricechildren = model.Pricechildren,
                    Pricesale = model.Pricesale,
                    Bed = model.Bed,
                    Cabingallery = model.Cabingallery,
                    Price = model.Price,
                    Content = model.Content,
                    MaxAdults = model.MaxAdults,
                    IDCruise = model.IDCruise,
                    Size = model.Size,
                    Image = model.Image,
                    Description = model.Description,
                };
                if (!string.IsNullOrEmpty(model.Price))
                {
                    var listtabcruise = db.Cruisetabs.Where(a => a.IDCruise == idcruise && a.Price != 0).ToList();
                    var listtab = JsonConvert.DeserializeObject<List<EPricetabCabin>>(model.Price);
                    var listtabold = new List<EPricetabCabin>();
                    foreach (var i in listtabcruise)
                    {
                        var tabsever = listtab.FirstOrDefault(a => a.IDTabCruise == i.ID);
                        if (tabsever != null)
                        {
                            var tab = new EPricetabCabin
                            {
                                IDTabCruise = i.ID,
                                NameTabCruise = i.Name,
                                Price = tabsever.Price,
                                Pricechildren = tabsever.Pricechildren,
                                Pricesale = tabsever.Pricesale
                            };
                            listtabold.Add(tab);
                        }
                        else
                        {
                            var tab = new EPricetabCabin
                            {
                                IDTabCruise = i.ID,
                                NameTabCruise = i.Name,
                                Price = 0,
                                Pricechildren = 0,
                                Pricesale = 0
                            };
                            listtabold.Add(tab);
                        }

                    }
                    ecabin.listcruitab = listtabold;
                }
                else
                {
                    var listtab = new List<EPricetabCabin>();
                    var listtabcruise = db.Cruisetabs.Where(a => a.IDCruise == idcruise && a.Price != 0).ToList();
                    foreach (var i in listtabcruise)
                    {
                        var tab = new EPricetabCabin
                        {
                            IDTabCruise = i.ID,
                            NameTabCruise = i.Name,
                            Price = 0,
                            Pricechildren = 0,
                            Pricesale = 0
                        };
                        listtab.Add(tab);
                    }
                    ecabin.listcruitab = listtab;
                }

                return View(ecabin);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(FormCollection frm)
        {
            try
            {
                var model = new Cabin
                {
                    Name = frm["Name"],
                    Bed = frm["Bed"],
                    //Cabingallery = "",
                    Content = frm["Content"],
                    IDCruise = Int32.Parse(frm["IDCruise"]),
                    Price = frm["Price"],
                    Pricesale = "",
                    Pricechildren = "",
                    MaxAdults = Int32.Parse(frm["MaxAdults"]),
                    Size = float.Parse(frm["Size"]),
                    Description = frm["Descrip"],
                    Image = frm["Image"]
                };
                var listtab = db.Cruisetabs.Where(a => a.IDCruise == Int32.Parse(frm["IDCruise"]) && a.Price != 0).ToList();
                var listprice = new List<EPricetabCabin>();
                foreach (var i in listtab)
                {
                    var price = new EPricetabCabin
                    {
                        IDTabCruise = i.ID,
                        NameTabCruise = i.Name,
                        Price = float.Parse(frm["Price-" + i.ID]),
                        Pricechildren = float.Parse(frm["Pricechildren-" + i.ID]),
                        Pricesale = float.Parse(frm["Pricesale-" + i.ID]),
                    };
                    listprice.Add(price);
                }
                model.Price = JsonConvert.SerializeObject(listprice);
                db.Cabins.InsertOnSubmit(model);
                db.SubmitChanges();

                return Json(new { success = true, id = model.ID });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateCabin(FormCollection frm)
        {
            try
            {
                int id = Int32.Parse(frm["IDCabin"]);
                Cabin model = db.Cabins.FirstOrDefault(a => a.ID == id);
                if (model == null)
                {
                    return Json(new { success = false });
                }
                model.Name = frm["Name"];
                model.Bed = frm["Bed"];
                //model.Cabingallery = "";
                model.Content = frm["Content"];
                model.IDCruise = Int32.Parse(frm["IDCruise"]);
                model.Price = frm["Price"];
                model.Pricesale = "";
                model.Pricechildren = "";
                model.MaxAdults = Int32.Parse(frm["MaxAdults"]);
                model.Size = float.Parse(frm["Size"]);
                model.Description = frm["Descrip"];
                model.Image = frm["Image"];
                var listtab = db.Cruisetabs.Where(a => a.IDCruise == Int32.Parse(frm["IDCruise"]) && a.Price != 0).ToList();
                var listprice = new List<EPricetabCabin>();
                foreach (var i in listtab)
                {
                    var price = new EPricetabCabin
                    {
                        IDTabCruise = i.ID,
                        NameTabCruise = i.Name,
                        Price = float.Parse(frm["Price-" + i.ID]),
                        Pricechildren = float.Parse(frm["Pricechildren-" + i.ID]),
                        Pricesale = float.Parse(frm["Pricesale-" + i.ID]),
                    };
                    listprice.Add(price);
                }
                model.Price = JsonConvert.SerializeObject(listprice);
                db.SubmitChanges();
                return Json(new { success = true, id = model.ID });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }

        }
        public JsonResult Listgalleryfcabin(string idcabin)
        {
            try
            {
                var listcabin = db.Cabins.FirstOrDefault(a => a.ID.ToString() == idcabin);
                return Json(new { data = listcabin.Cabingallery, success = true, JsonRequestBehavior.AllowGet });
            }
            catch
            {
                return Json(new { success = false, JsonRequestBehavior.AllowGet });
            }

        }
        [HttpPost]
        public JsonResult AddgalleryPhoto(int id, string name)
        {
            Cabin model = db.Cabins.FirstOrDefault(a => a.ID == id);
            if (model != null)
            {
                List<CruiseGallery> listDetail = new List<CruiseGallery>();

                var images = new CruiseGallery
                {
                    NameImages = name
                };
                if (string.IsNullOrEmpty(model.Cabingallery))
                {

                    listDetail.Add(images);
                    var addgallery = JsonConvert.SerializeObject(listDetail);
                    model.Cabingallery = addgallery;
                }
                else
                {
                    listDetail = JsonConvert.DeserializeObject<List<CruiseGallery>>(model.Cabingallery);
                    listDetail.Add(images);
                    model.Cabingallery = JsonConvert.SerializeObject(listDetail);
                }
                db.SubmitChanges();

            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult DeletePhoto(int id, string name)
        {
            var cabin = db.Cabins.FirstOrDefault(a => a.ID == id);
            if (cabin != null)
            {
                List<CruiseGallery> listDetail = new List<CruiseGallery>();
                listDetail = JsonConvert.DeserializeObject<List<CruiseGallery>>(cabin.Cabingallery);
                var model = listDetail.FirstOrDefault(a => a.NameImages == name);
                listDetail.Remove(model);
                cabin.Cabingallery = JsonConvert.SerializeObject(listDetail);
                db.SubmitChanges();
            }
            return Json(new { success = true });
        }

    }
}
