using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class OrderDetailViewModel
    {
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }

        [Display(Name = "Order Detail Number")]
        public int OrderDetailId { get; set; }

        [Display(Name = "Module Number")]
        public int ModuleId { get; set; }

        public Module Module { get; set; }

        public Promotion Promotion { get; set; }
    }
}
