using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using System.Linq;
using System;
using System.Net;

namespace TeamplateHotel.Controllers
{
    public class CTripController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(Models.Custom_Trip field, FormCollection formCollection)
        {
            MyDbDataContext db = new MyDbDataContext();
            string fullname = field.Fullname;
            string phone = field.Phone;
            string email = field.Email;
            string address = field.Address;
            string country = field.Country;
            DateTime departureDate = field.DepartureDate;
            string needVNvisa = formCollection["NeedVNVisa"];
            string totalleng = formCollection["daytrip"];
            string HotelGrande = formCollection["hotelstar"];
            string NumberOfClient = formCollection["NumberOfClient"];
            string VN = formCollection["Vn"];
            string Cam = formCollection["Cam"];
            string Laos = formCollection["Laos"];
            string request = field.OtherRequest;
            string describe = field.Describe;
            string socialMedia = field.SocialMedia;

            CustomizeTrip custom_trip = new CustomizeTrip();
            custom_trip.FullName = fullname;
            custom_trip.Phone = phone;
            custom_trip.SocialMedia = socialMedia;
            custom_trip.Address = address;
            custom_trip.Country = country;
            custom_trip.Email = email;
            custom_trip.DepartureDate = departureDate;
            custom_trip.NeedVietnamVisa = bool.Parse(needVNvisa);
            custom_trip.TotalLength = totalleng;
            custom_trip.NumberOfClients = NumberOfClient;
            custom_trip.HotelGrade = HotelGrande;
            custom_trip.VietNam = VN;
            custom_trip.Cambodia = Cam;
            custom_trip.Lao = Laos;
            custom_trip.OtherRequest = request;
            custom_trip.Describe = describe;
            custom_trip.IPAdress = GetIPAddress();

            db.CustomizeTrips.InsertOnSubmit(custom_trip);

            try
            {
                db.SubmitChanges();

                ViewBag.Email = custom_trip.Email;
                ViewBag.Name = custom_trip.FullName;

                SendEmail sendEmail =
                           db.SendEmails.FirstOrDefault(
                               a => a.Type == TypeSendEmail.CustomTrip && a.LanguageID == "en");

                sendEmail.Title = sendEmail.Title.Replace("{NameHotel}", "System");
                string content = sendEmail.Content;
                content = content.Replace("{FullName}", custom_trip.FullName);
                content = content.Replace("{Email}", custom_trip.Email);
                content = content.Replace("{Phone}", custom_trip.Phone);
                content = content.Replace("{Country}", custom_trip.Country);
                content = content.Replace("{SocialMedia}", custom_trip.SocialMedia);

                content = content.Replace("{DepartureDate}", custom_trip.DepartureDate != null ? ((DateTime)custom_trip.DepartureDate).ToString("MMMM dd yyyy") : "");
                content = content.Replace("{TotalLength}", custom_trip.TotalLength);
                content = content.Replace("{HotelGrade}", custom_trip.HotelGrade);
                content = content.Replace("{NeedVietnamVisa}", custom_trip.NeedVietnamVisa == true ? "Yes" : "No");
                content = content.Replace("{NumberOfClients}", custom_trip.NumberOfClients);
                content = content.Replace("{VietNam}", custom_trip.VietNam);
                content = content.Replace("{Lao}", custom_trip.Lao);
                content = content.Replace("{Cambodia}", custom_trip.Cambodia);
                content = content.Replace("{OtherRequest}", custom_trip.OtherRequest);
                content = content.Replace("{Describe}", custom_trip.Describe);

                if (MailHelper.SendMail(custom_trip.Email, sendEmail.Title, content))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Messages", "CTrip", new { status = "Erros Send Mail" });
                }

            }
            catch (Exception e)
            {
                string s = e.Message;
                return RedirectToAction("Messages", "CTrip", new { status = s });
            }
        }
        public string GetIPAddress()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            string IPAddress = "";
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
        [HttpGet]
        public ActionResult Messages()
        {
            using (var db = new MyDbDataContext())
            {
                SendEmail sendEmail =
                       db.SendEmails.FirstOrDefault(
                           a => a.Type == TypeSendEmail.CustomTrip && a.LanguageID == Request.Cookies["LanguageID"].Value);

                string status = Request.Params["status"];
                ViewBag.Messages = "";
                if (string.IsNullOrEmpty(status) == false)
                {
                    if (status.Equals("success"))
                    {
                        ViewBag.Messages = sendEmail.Success;
                    }
                    else
                    {
                        ViewBag.Messages = sendEmail.Error + status;
                    }
                }
                else
                {
                    ViewBag.Messages = sendEmail.Error + status;
                }
                return View();
            }
        }
    }

}