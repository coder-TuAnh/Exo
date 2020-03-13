using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectLibrary.Database;
using ProjectLibrary.Security;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            using (var db = new MyDbDataContext())
            {
                //string cookieClient = Request.Cookies["name_client"].Value;
                //string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
                //string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
                //var user = db.Users.FirstOrDefault(a => a.UserName == userName);
                //if(user.UserContent== true)
                //{
                //    int cout = 0;
                //    HttpCookie langCookie = Request.Cookies["lang_client"];
                //    while (langCookie != null)
                //    {
                //        langCookie.Expires = DateTime.Now.AddDays(-30);
                //        HttpContext.Response.Cookies.Add(langCookie);
                //        cout++;
                //        if (cout == 10)
                //            break;
                //    }
                //    cout = 0;
                //    HttpCookie nameCookie = Request.Cookies["name_client"];
                //    while (nameCookie != null)
                //    {
                //        nameCookie.Expires = DateTime.Now.AddDays(-30);
                //        HttpContext.Response.Cookies.Add(nameCookie);
                //        cout++;
                //        if (cout == 10)
                //            break;
                //    }
                //    CurrentSession.ClearAll();
                //    return Redirect("http://swallowtravel.com/admin");
                //}
                ViewBag.Title = "List user";
                ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
                return View();

            }
                
        }

    [HttpPost]
    public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
    {
        try
        {
            using (var db = new MyDbDataContext())
            {
                //List<EUser> records = db.Users.Select(a => new EUser
                //{
                //    ID = a.ID,
                //    UserName = a.UserName,
                //    Password = a.Password,
                //    FullName = a.FullName,
                //    Email = a.Email,
                //    Status = (bool)a.Status,
                //    UserContent = a.UserContent ?? false,
                //}).ToList();
                    var user = from a in db.Users
                               select new EUser
                               {
                                   ID = a.ID,
                                   UserName = a.UserName,
                                   Password = a.Password,
                                   FullName = a.FullName,
                                   Email = a.Email,
                                   Status = (bool)a.Status,
                                   UserContent = a.UserContent ?? false,
                               };
                //Return result to jTable
                return Json(new { Result = "OK", Records = user.ToList(), TotalRecordCount = user.ToList().Count });
            }

        }

        catch (Exception ex)
        {
            return Json(new { Result = "ERROR", message = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult Create(EUser model)
    {
        using (var db = new MyDbDataContext())
        {

            User checkUserName = db.Users.FirstOrDefault(a => a.UserName == model.UserName);
            if (checkUserName != null)
            {
                ModelState.AddModelError("UserName",
                    "Username is exist");
            }
            User checkEmail = db.Users.FirstOrDefault(a => a.Email == model.Email);
            if (checkEmail != null)
            {
                ModelState.AddModelError("Email",
                    "Email is exist");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var insert = new User
                    {
                        UserName = model.UserName,
                        Password = Password.HashPassword(model.Password),
                        FullName = model.FullName,
                        Email = model.Email,
                        PasswordOld = model.Password,
                        //PasswordOld = CryptorEngine.Encrypt(model.Password, true),
                        Status = model.Status,
                        UserContent = model.UserContent,
                    };

                    db.Users.InsertOnSubmit(insert);
                    db.SubmitChanges();
                    model.ID = insert.ID;

                    return Json(new { Result = "OK", Message = "Successful", Record = model });
                }
                catch (Exception exception)
                {
                    return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                }
            }
            return
                Json(
                    new
                    {
                        Result = "Error",
                        Errors = ModelState.Errors(),
                        Message = "error data"
                    });
        }
    }

    [HttpPost]
    public ActionResult Edit(EUser model)
    {
        using (var db = new MyDbDataContext())
        {
            if (ModelState.IsValid)
            {
                //check username đã tồn tại chưa
                IQueryable<User> checkUserName = db.Users.Where(a => a.UserName == model.UserName);
                if (checkUserName.Count() > 1)
                {
                    ModelState.AddModelError("UserName",
                        "user name is exist");
                }

                //check email đã tồn tại chưa
                IQueryable<User> checkEmail = db.Users.Where(a => a.Email == model.Email);
                if (checkEmail.Count() > 1)
                {
                    ModelState.AddModelError("Email",
                        "Email is exist");
                }

                User edit = db.Users.FirstOrDefault(b => b.ID == model.ID);
                if (edit == null)
                {
                    TempData["Messages"] = "username not exist";
                    return RedirectToAction("Index");
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    model.Password = edit.Password;
                    ModelState.Remove("Password");
                }

                try
                {
                    edit.UserName = model.UserName;
                    edit.FullName = model.FullName;
                    edit.Email = model.Email;
                    edit.Status = model.Status;
                    edit.UserContent = model.UserContent;

                    if (model.Password != edit.Password)
                    {
                        edit.Password = Password.HashPassword(model.Password);
                            edit.PasswordOld = model.Password;
                        //edit.PasswordOld = CryptorEngine.Encrypt(model.Password, true);
                    }
                    db.SubmitChanges();

                    return Json(new { Result = "OK", Message = "Insert successful" });
                }
                catch (Exception exception)
                {
                    return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                }
            }
            return
                Json(
                    new
                    {
                        Result = "Error",
                        Errors = ModelState.Errors(),
                        Message = "error data"
                    });
        }
    }

    [HttpPost]
    public JsonResult Delete(int id)
    {
        try
        {
            using (var db = new MyDbDataContext())
            {
                User del = db.Users.FirstOrDefault(c => c.ID == id);
                if (del != null)
                {
                    //Xóa người dùng này
                    db.Users.DeleteOnSubmit(del);
                    db.SubmitChanges();
                    return Json(new { Result = "OK", Message = "Successful" });
                }
                return Json(new { Result = "ERROR", Message = "Username not exist" });
            }
        }
        catch (Exception ex)
        {
            return Json(new { Result = "Error", Message = "Error: " + ex.Message });
        }
    }
}
}