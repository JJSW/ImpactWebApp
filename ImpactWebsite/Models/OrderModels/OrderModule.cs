using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class OrderModule
    {
        [Key]
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int DeliveryDays { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public int UnitPriceId { get; set; }
        public UnitPrice UnitPrice { get; set; }
        [Display(Name = "Unit Price")]
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
