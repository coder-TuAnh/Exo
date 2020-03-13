using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary.Config
{
    public class ModulePermission
    {
        public const int ConfigSystem = 100;
        public const int ConfigEmail = 101;
        public const int ConfigPayment = 102;
        public const int ConfigMenuSystem = 103;
        public const int ConfigHotel = 104;

        public const int System = 200;
        public const int Language = 201;
        public const int Hotel = 202;
        public const int InfoHotelByLanguage = 204;
        public const int User = 203;


        public const int ManagerHotel = 300;
        public const int Advertise = 301;
        public const int SocialNetWork = 302;
        public const int OnlineSupport = 303;
        public const int Tripadvisor = 304;

        public const int ImageAndVideo = 400;
        public const int Banner = 401;
        public const int Gallery = 402;
        public const int Video = 403;

        public const int ManagerMenu = 500;
        public const int Menu = 501;

        public const int ManagerContent = 600;
        public const int Article = 601;
        public const int Room = 602;
        public const int Tour = 603;

        public const int PromotionAndService = 700;
        public const int Discount = 701;
        public const int Service = 702;

        public const int ConfigRoom = 800;
        public const int SetupRoom = 801;

        public const int Booking = 900;
        public const int TourViaEmail = 901;
        public const int TourOnline = 902;

        public const int Contact = 1000;



        //
        public static Dictionary<int, string> ListPermissions = new Dictionary<int, string>()
                {
                    {ConfigSystem,"Cấu hình hệ thống"},
                    {ConfigEmail,"Cấu hình email"},
                    {ConfigHotel,"Cấu hình khách sạn"},
                    {ConfigPayment,"Cấu hình thanh toán"},
                    {ConfigMenuSystem,"Menu System"},

                    {System,"Quản lý hệ thống"},
                    {Language,"Ngôn ngữ"},
                    {Hotel,"Khách sạn"},
                     {InfoHotelByLanguage,"Thông tin khách sạn theo ngôn ngữ"},
                    {User,"Người dùng"},

                    {ManagerHotel,"Quản lý khách sạn"},
                    {Advertise,"Quảng cáo"},
                    {SocialNetWork,"Mạng xã hội"},
                    {OnlineSupport,"Hỗ trợ trực tuyến"},
                    {Tripadvisor,"Tripadvisod"},

                    {ImageAndVideo,"Quản lý hình ảnh & Video"},
                    {Banner,"Banner"},
                    {Gallery,"Gallery"},
                    {Video,"Video"},

                    {ManagerMenu,"Quản lý menu"},
                    {Menu,"Menu"},

                    {ManagerContent,"Quản lý nội dung"},
                    {Article,"Bài viết"},
                    {Room,"Phòng"},
                    {Tour,"Tour"},

                    {PromotionAndService,"Giảm giá & dịch vụ"},
                    {Discount,"Giảm giá"},
                    {Service,"Dịch vụ"},

                    {ConfigRoom,"Config room"},
                    {SetupRoom,"Cài đặt phòng" +
                               ""},

                    {Booking,"Quản lý booking"},
                    {TourViaEmail,"Booking tour via email"},
                    {TourOnline,"Booking tour online"},

                    {Contact,"Contact"},

                };
    }
}
