using System.Collections.Generic;

namespace TeamplateHotel.Models
{
    public class PagingOject
    {
        public List<ShowObject> ListObject { get; set; }
        public int NumberObjectInPage { get; set; }
        public int TotalPage { get; set; }
        public int Page { get; set; }
    }
}