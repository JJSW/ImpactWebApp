using ImpactWebsite.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class OrderDetail : BaseEntity
    {
        [Key]
        [Display(Name = "Order Detail Number")]
        public int OrderDetailId { get; set; }

        [Display(Name = "Order Number")]
        public int OrderId {get;set;}

        [Display(Name = "Module Number")]
        public int ModuleId { get; set; }

        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }


        public Module Module { get; set; }
    }
}
