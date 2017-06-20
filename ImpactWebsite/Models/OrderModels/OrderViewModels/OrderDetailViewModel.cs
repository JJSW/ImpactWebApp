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
        public Int32 OrderId { get; set; }

        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Order Detail Number")]
        public Int32 OrderDetailId { get; set; }

        [Display(Name = "Module Number")]
        public Int32 ModuleId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Module Price")]
        public int UnitPrice { get; set; }

        [Display(Name = "Total Amount")]
        public int TotalAmount { get; set; }

        [Display(Name = "Billing Address Number")]
        public Int32 BillingAddressId { get; set; }

        public Module Module { get; set; }

        public Promotion Promotion { get; set; }

        public Investment Investment { get; set; }
    }
}
