using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using TeamplateHotel.Models;

namespace TeamplateHotel.Controllers
{
    public class HomeController : BasicController
    {
        [HttpGet]
        public ActionResult Index(object aliasMenuSub, object idSub, object aliasSub)

        {
            string lan = Request.Cookies["LanguageID"].Value;
            var db = new MyDbDataContext();
            Hotel hotel = CommentController.DetailHotel(lan);
            ViewBag.MetaTitle = hotel.MetaTitle;
            ViewBag.MetaDesctiption = hotel.MetaDescription;

            if (aliasMenuSub.ToString() == "System.Object")
            {
                return View("Index");
            }
            if (aliasMenuSub.ToString() == "Search")
            {
                string key = Request.Params["key"];
                if (string.IsNullOrEmpty(key))
                {
                    return View("Tour/Search", new List<Tour>());
                }
                List<ShowObject> listSearch = new List<ShowObject>();
                listSearch.AddRange(db.Tours.Where(a => a.Status && a.Title.Contains(key)).OrderBy(a => a.Index).Select(a => new ShowObject() {
                    ID = a.ID,
                    Alias = a.Alias,
                    Description = a.Description,
                    Image = a.Image,
                    Index = a.Index,
                    MenuAlias = a.Alias,
                    Title = a.Title,
                }).ToList());
                listSearch.AddRange(db.Articles.Where(a => a.Status && a.Title.Contains(key)).OrderBy(a => a.Index).Select(a => new ShowObject()
                {
                    ID = a.ID,
                    Alias = a.Alias,
                    Description = a.Description,
                    Image = a.Image,
                    Index = a.Index,
                    MenuAlias = a.Alias,
                    Title = a.Title,
                }).ToList());
                
                return View("Tour/Search", listSearch);
            }
            if (aliasMenuSub.ToString() == "SelectLanguge")
            {
                Language language = db.Languages.FirstOrDefault(a => a.ID == idSub.ToString());
                if (language == null)
                {
                    language = db.Languages.FirstOrDefault();
                }
                HttpCookie langCookie = Request.Cookies["LanguageID"];
                langCookie.Value = language.ID;
                langCookie.Expires = DateTime.Now.AddDays(10);
                HttpContext.Response.Cookies.Add(langCookie);
                return Redirect("/");
            }

            // xác định menu => tìm ra Kiểu hiển thị của menu
            Menu menu = db.Menus.FirstOrDefault(a => a.Alias == aliasMenuSub.ToString());
            if (menu == null)
            {
                return View("404");
            }
            //Seo
            ViewBag.MetaTitle = menu.MetaTitle ?? menu.Title;
            ViewBag.MetaDesctiption = menu.MetaDescription ?? menu.Title;
            ViewBag.Menu = menu;

            switch (menu.Type)
            {
                //case SystemMenuType.Cruise:
                //    goto TrangCruise;
                case SystemMenuType.Article:
                    goto Trangbaiviet;
                case SystemMenuType.Tour:
                    goto TrangTour;
                //case SystemMenuType.RoomRate:
                //    goto TrangRoom;
                //case SystemMenuType.Service:
                //    goto Service;
                case SystemMenuType.Ctrip:
                    return RedirectToAction("Index", "CTrip");

                case SystemMenuType.Contact:
                    return View("Contact");

                case SystemMenuType.Hotel:
                    goto Trangbaiviet;
                //case SystemMenuType.About:
                //    return View("About");

                //case SystemMenuType.Nosvalue:
                //    return View("Nosvalue");

                //case SystemMenuType.Othertours:
                //    return View("OtherTours");

                //case SystemMenuType.HighLightTour:
                //    return View("Tour/ListHighLightTour", CommentController.Tourhots(lan));

                //case SystemMenuType.SpecialTour:
                //    goto SpecialTour;

                //case SystemMenuType.FeedBack:
                //    return View("FeedBack", CommentController.Getfeed());

                //case SystemMenuType.Gallery:
                //    return View("Gallery", CommentController.Gallery());

                //case SystemMenuType.Location:
                //    //Lấy bài viết Location
                //    ViewBag.ArticleByRoomRate = db.Articles.FirstOrDefault(a => a.MenuID == menu.ID);
                //    return View("Location");

                default:
                    return View("Index");
            }

            #region "Trang bài viết"

            Trangbaiviet:
            if (idSub.ToString() != "System.Object")
            {
                int idArticle;
                int.TryParse(idSub.ToString(), out idArticle);
                DetailArticle detailArticle = CommentController.Detail_Article(idArticle);
                ViewBag.MetaTitle = detailArticle.Article.MetaTitle;
                ViewBag.MetaDesctiption = detailArticle.Article.MetaDescription;
                return View("Article/DetailArticle", detailArticle);
            }
            //Danh sách bài viết
            List<Article> articles = CommentController.GetArticles(menu.ID);
            if (articles.Count == 1)
            {
                DetailArticle detailArticle = CommentController.Detail_Article(articles[0].ID);
                ViewBag.MetaTitle = detailArticle.Article.MetaTitle;
                ViewBag.MetaDesctiption = detailArticle.Article.MetaDescription;
                return View("Article/DetailArticle", detailArticle);
            }
            return View("Article/ListArticle", articles);

            #endregion "Trang bài viết"

            //Trường hợp: Room

            #region "Kiểu Room & rate"

            TrangRoom:
            if (idSub.ToString() != "System.Object")
            {
                int id;
                int.TryParse(idSub.ToString(), out id);
                //Kiểm tra xem alias truyền đến có phải là 1 bài viết không
                var articleRoom = db.Articles.FirstOrDefault(a => a.ID == id);
                if (articleRoom != null)
                {
                    ViewBag.MetaTitle = articleRoom.MetaTitle;
                    ViewBag.MetaDesctiption = articleRoom.MetaDescription;
                    return View("Article/DetailArticle", CommentController.Detail_Article(id));
                }
                //chi tiết Room
                DetailRoom detailRoom = CommentController.Detail_Room(id, menu.ID);
                ViewBag.MetaTitle = detailRoom.Room.MetaTitle;
                ViewBag.MetaDesctiption = detailRoom.Room.MetaDescription;

                return View("Room/DetailRoom", detailRoom);
            }
            //Lấy bài viết RoomRate
            var articlrByRoomRate = db.Articles.FirstOrDefault(a => a.MenuID == menu.ID);
            if (articlrByRoomRate != null)
            {
                articlrByRoomRate.MenuAlias = articlrByRoomRate.Alias;
            }
            ViewBag.ArticleByRoomRate = articlrByRoomRate;
            return View("Room/ListRoom", CommentController.GetRooms(menu.ID));

            #endregion "Kiểu Room & rate"

            //Trang Service

            #region "Trang Service"

            Service:
            if (idSub.ToString() != "System.Object")
            {
                int id;
                int.TryParse(idSub.ToString(), out id);
                DetailService detailService = CommentController.Detail_Service(id);
                ViewBag.MetaTitle = detailService.Service.MetaTitle;
                ViewBag.MetaDesctiption = detailService.Service.MetaDescription;
                return View("Service/DetailService", detailService);
            }

            List<Service> services = CommentController.GetServices(menu.ID);
            if (services.Count == 1)
            {
                DetailService detailService = CommentController.Detail_Service(services[0].ID);
                ViewBag.MetaTitle = detailService.Service.MetaTitle;
                ViewBag.MetaDesctiption = detailService.Service.MetaDescription;
                return View("Service/DetailService", detailService);
            }
            return View("Service/ListService", services);

            #endregion "Trang Service"

            //trường hợp: Tour

            #region "Kiếu tour"

            TrangTour:
            if (idSub.ToString() != "System.Object")
            {
                int idTour;
                int.TryParse(idSub.ToString(), out idTour);
                DetailTour detailTour = CommentController.Detail_Tour(idTour);
                ViewBag.MetaTitle = detailTour.Tour.MetaTitle ?? detailTour.Tour.Title;
                ViewBag.MetaDesctiption = detailTour.Tour.MetaDescription ?? detailTour.Tour.Title;
                if ((bool)detailTour.Tour.Deal)
                {
                    return View("Tour/DetailTourDeal", detailTour);
                }
                return View("Tour/DetailTour", detailTour);
            }
            if(aliasMenuSub.ToString()== "vietnam-business-visa" || aliasMenuSub.ToString()== "vietnam-tourist-visa")
            {

                var tour = from a in db.Tours
                           join b in db.Menus on a.MenuID equals b.ID
                           where b.Alias==aliasMenuSub.ToString()
                           select a;
                List<Tour> aa = tour.ToList();
                if (tour.Count() == 1)
                {
                    DetailTour detailTour = CommentController.Detail_Tour(aa[0].ID);
                    ViewBag.MetaTitle = detailTour.Tour.MetaTitle ?? detailTour.Tour.Title;
                    ViewBag.MetaDesctiption = detailTour.Tour.MetaDescription ?? detailTour.Tour.Title;
                    //if ((bool)detailTour.Tour.Deal)
                    //{
                    //    return View("Tour/DetailTourDeal", detailTour);
                    //}
                    return View("Tour/DetailTour", detailTour);
                }
                else
                {
                    return View("Tour/ListTour", CommentController.GetTours(menu.ID));
                }
               
            }
            return View("Tour/ListTour", CommentController.GetTours(menu.ID));

            #endregion "Kiếu tour"

            //trường hợp: Cruise

            #region "Kiếu Cruise"

            TrangCruise:
            ViewBag.MenuCruise = db.Menus.FirstOrDefault(a => a.ID == menu.ID).Title;
            return View("Cruise/ListCruise", CommentController.GetCruise(menu.ID));

            #endregion "Kiếu Cruise"
        }
    }
}