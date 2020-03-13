using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EMenuSystem
    {
        public int Id { get; set; }
        [Required]
        public int ParentId { get; set; }
        [Required]
        public string Name { get; set; }
       
        public string Icon { get; set; }
        [Required]
        public string Link{ get; set; }
        [Required]
        public int Level{ get; set; }
        [Required]
        public bool State{ get; set; }
         [Required]
        public int Role{ get; set; }
        [Required]
        public bool Hot{ get; set; }
        
    }
}