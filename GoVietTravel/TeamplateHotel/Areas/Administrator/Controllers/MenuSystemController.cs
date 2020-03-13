using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.ModelShow;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class MenuSystemController : Controller
    {
        //
        // GET: /Administrator/MenuSystem/

        public ActionResult Index()
        {
            return View();
        }
         
        [HttpPost]
        public JsonResult ListMenu(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                { 
                    var listMenu = from m in db.MenuSystems
                                   select new
                                   {

                                       Id = m.Id,
                                       Name = m.Name,
                                        
                                   };
                    listMenu.ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = listMenu, TotalRecordCount = listMenu.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }
}
