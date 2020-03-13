using System.Collections.Generic;

namespace ProjectLibrary.Config
{
    public class SystemMenuLocation
    {
        public const int MainMenu = 1;
        public const int SecondMenu = 3;
        public const int SecondMenuTop = 4;

        public int LocationId { get; set; }
        public string AliasMenu { get; set; }
        public string TitleMenu { get; set; }
        

        public static  List<SystemMenuLocation> ListLocationMenu()
        {
            var listLocationMenu = new List<SystemMenuLocation>
                                       {
                                           new SystemMenuLocation
                                               {
                                                   LocationId = MainMenu,
                                                   TitleMenu = "Main menu",
                                                   AliasMenu = "MainMenu"
                                               },
                                           new SystemMenuLocation
                                               {
                                                   LocationId = SecondMenu,
                                                   TitleMenu = "Footer Menu",
                                                   AliasMenu = "SecondMenu"
                                               },
                                           new SystemMenuLocation
                                               {
                                                   LocationId = SecondMenuTop,
                                                   TitleMenu = "Top Menu",
                                                   AliasMenu = "SecondMenuTop"
                                               },
                                       };

            return listLocationMenu;
        }
    }

}
