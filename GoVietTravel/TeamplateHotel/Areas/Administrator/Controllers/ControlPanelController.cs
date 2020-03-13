using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectLibrary.Database;
using ProjectLibrary.Security;


namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class ControlPanelController : BaseController
    {
        //
        // GET: /Administrator/ControlPanel/
        public ActionResult Index()
        {
            var db = new MyDbDataContext();
            string cookieClient = Request.Cookies["name_client"].Value;
            string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
            string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
            ViewBag.Users = db.Users.FirstOrDefault(a => a.UserName == userName);
            ViewBag.Title = "Administrator";
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            return View();
        }

    }
}
