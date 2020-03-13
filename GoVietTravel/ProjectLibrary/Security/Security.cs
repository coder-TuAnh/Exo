using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectLibrary.Config;
using ProjectLibrary.Database;

namespace ProjectLibrary.Security
{
    public class Security
    {
        //public static bool Authorization(int userId, int role)
        //{
        //    using (var db = new MyDbDataContext())
        //    {
        //        if(CurrentSession.IsAdmin)
        //        {
        //            return true;
        //        }
        //        try
        //        {
        //            var checkRole = db.Roles.Where(a=>a.UserId==userId).ToList().Join(db.MenuSystems.Where(a=>a.Role == role).ToList(),a=>a.MenuId, b=>b.Id,(a,b)=> new {a,b}).ToList();
        //            if(checkRole.Count>0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                CurrentSession.ClearAll();
        //                return false;
        //            }
        //        }
        //        catch
        //        {
        //            CurrentSession.ClearAll();
        //            return false;
        //        }
        //    }
        //}
    }
}
