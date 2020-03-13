using System.Collections.Generic;

namespace ProjectLibrary.Config
{
    public class SystemMenuType
    {
        public const int Home = 1;
        public const int Article = 2;
        public const int Tour = 6;
        public const int Hotel = 5;
        public const int Contact = 7;
        public const int OutSite = 17;
        public const int Ctrip = 8;
        public const int Visa = 4;
        //public const int Visa1 = 4;


        public static Dictionary<int, string> CategoryType = new Dictionary<int, string>()
                                                                 {
                                                                     {Home, "Home page"},
                                                                     {Article, "Chuyên mục bài viết"},
                                                                     {Tour, "Chuyên mục tour"},
                                                                      {Hotel, "Chuyên mục hotel"},
                                                                     {Contact, "Contact"},
                                                                     {OutSite, "Out Site"},
                                                                     {Ctrip, "Chuyến khác"},
                                                                      //{Visa, "Visa"},
                                                                       {Visa, "Visa"},
                                                                 };
    }
}