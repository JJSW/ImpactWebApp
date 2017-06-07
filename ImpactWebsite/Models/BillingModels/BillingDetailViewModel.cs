using ImpactWebsite.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.BillingModels
{
    public class BillingDetailViewModel
    {
        [Display(Name = "Order Number")]
        public Int32 OrderId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Display(Name = "Module Number")]
        public Int32 ModuleId { get; set; }

        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        [Display(Name = "Module Names")]
        public string ModuleNames { get; set; }

        [Display(Name = "Module Price")]
        public int UnitPrice { get; set; }

        public int TotalAmount { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatusList OrderStatus { get; set; }

        [Display(Name = "Billing Address Number")]
        public Int32 BillingAddressId { get; set; }
    }
}
