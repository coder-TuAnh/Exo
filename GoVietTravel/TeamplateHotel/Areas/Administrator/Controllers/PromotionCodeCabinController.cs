using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class PromotionCodeCabinController : Controller
    {
        //
        // GET: /Administrator/PromotionCodeCabin/

        //
        // GET: /Administrator/PromotionCode/
        private MyDbDataContext db = new MyDbDataContext();
        public ActionResult Index()
        {
            return View();
        }

        // lay ra danh sach ma
        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var list = db.PromotionCodeCabins.Select(a => new
                {
                    a.ID,
                    a.Code,
                    a.NgayBatDau,
                    a.NgayKetThuc,
                    a.SL,
                    a.DaDung,
                    a.IDCabin,
                    a.Cabin.Name,
                    a.TrangThai,
                }).OrderBy(a => a.NgayKetThuc).ToList();
                return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        // chi tiet ma giam gia
        public ActionResult AddPromotion()
        {
            Session["TrungMa"] = "TrungMa";
            List<SelectListItem> listCruises = new List<SelectListItem>();
            listCruises.Add(new SelectListItem() { Value = "0", Text = "--Chọn Tàu--" });
            foreach (var b in db.Cruises.Select(a => new { a.ID, a.Name }))
            {
                listCruises.Add(new SelectListItem() { Value = b.ID.ToString(), Text = b.Name });
            }
            ViewBag.ListMenuID = new SelectList(listCruises, "Value", "Text");


            //ViewBag.listDay = GetListDay();
            ViewBag.cmd = "Create";
            var PromotionCode = new EntityModel.EPromotioncodeCabin();
            return View(PromotionCode);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = new ProjectLibrary.Database.PromotionCodeCabin();
                    model.Code = frm["txtCode"];
                    string[] str = frm["txtNgayBatDau"].ToString().Split('/', '-');
                    int day = int.Parse(str[0]);
                    int month = int.Parse(str[1]);
                    int year = int.Parse(str[2]);
                    model.NgayBatDau = new DateTime(year, month, day);
                    str = frm["txtNgayKetThuc"].ToString().Split('/', '-');
                    day = int.Parse(str[0]);
                    month = int.Parse(str[1]);
                    year = int.Parse(str[2]);
                    model.NgayKetThuc = new DateTime(year, month, day);
                    model.IDCabin = int.Parse(frm["txtIDCabin"].ToString());
                    model.IDCruise = int.Parse(frm["txtIDCruise"].ToString());
                    model.SL = int.Parse(frm["txtSL"].ToString());
                    model.DaDung = int.Parse(frm["txtDaDung"].ToString());
                    model.MoTa = frm["txtMoTa"].ToString();
                    model.TrangThai = bool.Parse(frm["txtTrangThai"].ToString());

                    Session["TrungMa"] = "";
                    List<ProjectLibrary.Database.PromotionCodeCabin> del = db.PromotionCodeCabins.Where(c => c.Code == model.Code && c.IDCabin == model.IDCabin).ToList();
                    if (del.Count < 1)
                    {
                        db.PromotionCodeCabins.InsertOnSubmit(model);
                        db.SubmitChanges();
                        Session["TrungMa"] = "Thêm Thành Công!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session["TrungMa"] = "TrungMa";
                        return RedirectToAction("AddPromotion");
                    }

                }

                catch (Exception ex)
                {
                    
                    return Json(new { success = false, Message = ex.Message });
                }
            }
            return RedirectToAction("AddPromotion");

        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<SelectListItem> listCruises = new List<SelectListItem>();
            listCruises.Add(new SelectListItem() { Value = "0", Text = "--Chọn Tàu--" });
            foreach (var b in db.Cruises.Select(a => new { a.ID, a.Name }))
            {
                listCruises.Add(new SelectListItem() { Value = b.ID.ToString(), Text = b.Name });
            }
            ViewBag.ListMenuID = new SelectList(listCruises, "Text", "Value");
            var code = db.PromotionCodeCabins.FirstOrDefault(x => x.ID == id);
            var Cabin = db.Cabins.FirstOrDefault(x => x.ID == code.IDCabin);

            // lay cabin hien tai
            ViewBag.IDCabin = Cabin.ID;
            ViewBag.Name = Cabin.Name;

            ViewData["code"] = code;
            return View();
        }
        public ActionResult UpdateCode(FormCollection frm)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int id = int.Parse(frm["txtID"].ToString());
                    var model = db.PromotionCodeCabins.FirstOrDefault(c => c.ID == id);


                    if (model != null)
                    {

                        model.Code = frm["txtCode"].ToString();
                        string[] str = frm["txtNgayBatDau"].ToString().Split('/', '-');
                        int day = int.Parse(str[0]);
                        int month = int.Parse(str[1]);
                        int year = int.Parse(str[2]);
                        model.NgayBatDau = new DateTime(year, month, day);
                        str = frm["txtNgayKetThuc"].ToString().Split('/', '-');
                        day = int.Parse(str[0]);
                        month = int.Parse(str[1]);
                        year = int.Parse(str[2]);
                        model.NgayKetThuc = new DateTime(year, month, day);
                        model.IDCabin = int.Parse(frm["txtIDCabin"].ToString());
                        model.IDCruise = int.Parse(frm["txtIDCruise"].ToString());
                        model.SL = int.Parse(frm["txtSL"].ToString());
                        model.DaDung = int.Parse(frm["txtDaDung"].ToString());
                        model.MoTa = frm["txtMoTa"].ToString();
                        string a = frm["txtTrangThai"].ToString();
                        model.TrangThai = bool.Parse(a);
                        db.SubmitChanges();
                        Session["TrungMa"] = "Update Thành Công!";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        Session["TrungMa"] = "TrungMa";
                        return RedirectToAction("Edit");
                    }

                }

                catch (Exception ex)
                {
                    return Json(new { success = false ,Message=ex.Message});
                }
            }
            return RedirectToAction("UpdateCode");
        }

        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    ProjectLibrary.Database.PromotionCodeCabin del = db.PromotionCodeCabins.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        //xóa hết hình ảnh của phòng này
                        db.PromotionCodeCabins.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa mã thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "mã không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        // lay danh sach cabin theo ID Cruise
        [HttpGet]
        public JsonResult GetListCabin(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<SelectListItem> listmenu = new List<SelectListItem>();
                    foreach (var b in db.Cabins.Select(a => new { a.ID, a.IDCruise, a.Name }).Where(a => a.IDCruise == id).ToList())
                    {
                        listmenu.Add(new SelectListItem() { Value = b.ID.ToString(), Text = b.Name });
                    }
                    return Json(new { listmenu }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult GetListAll()
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<SelectListItem> listmenu = new List<SelectListItem>();
                    foreach (var b in db.Cabins.Select(a => new { a.ID, a.IDCruise, a.Name }).ToList())
                    {
                        listmenu.Add(new SelectListItem() { Value = b.ID.ToString(), Text = b.Name });
                    }
                    return Json(new { listmenu }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        [HttpPost]
        public JsonResult CheckMa(string code, int IDCabin)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    DateTime today = DateTime.Now;
                    ProjectLibrary.Database.PromotionCodeCabin del = db.PromotionCodeCabins.FirstOrDefault(c => c.Code == code && c.IDCabin == IDCabin
                    && today <= c.NgayKetThuc && today >= c.NgayBatDau && c.TrangThai == true);
                    if (del != null)
                    {
                        /*
                         1-het ma
                         2-ok co ma va con luot dung
                         3-loi ko co ma nao
                         */
                        if (del.DaDung >= del.SL)
                        {
                            return Json(new { Result = "OK", Message = "1" });
                        }
                        else
                        {
                            del.DaDung = del.DaDung + 1;
                            db.SubmitChanges();
                            return Json(new { Result = "OK", Message = "2" });
                        }

                    }
                    return Json(new { Result = "ERROR", Message = "3" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }


    }
}
