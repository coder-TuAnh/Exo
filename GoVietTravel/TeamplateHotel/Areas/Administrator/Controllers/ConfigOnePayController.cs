using System;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Database;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class ConfigOnePayController : BaseController
    {
        [HttpGet]
        public ActionResult Config()
        {
            ViewBag.Title = "Thêm Payment Config OnePay";
            using (var db = new MyDbDataContext())
            {
                PaymentConfigOnePay Config = db.PaymentConfigOnePays.FirstOrDefault();
                return View(Config);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateConfig(PaymentConfigOnePay model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        PaymentConfigOnePay Config = db.PaymentConfigOnePays.FirstOrDefault();
                        if (Config == null)
                        {
                            Config = new PaymentConfigOnePay();
                            db.PaymentConfigOnePays.InsertOnSubmit(Config);
                            db.SubmitChanges();
                            ViewBag.Messages = "Thêm Payment Config OnePay thành công";
                            return RedirectToAction("GoogleAnalytics");
                        }
                        Config.AccessCode = model.AccessCode;
                        Config.Hashcode = model.Hashcode;
                        Config.MerchantId = model.MerchantId;
                        Config.WebSite = model.WebSite;

                        db.SubmitChanges();
                        ViewBag.Messages = "Chỉnh sửa Payment Config OnePay thành công";
                        return RedirectToAction("Config");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Lỗi: " + exception.Message;
                        return View();
                    }
                }
                return View(model);
            }
        }
    }
}