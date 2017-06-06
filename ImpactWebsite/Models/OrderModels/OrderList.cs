using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class OrderList
    {
        public bool IsChecked { get; set; }
        public OrderModule Modules { get; set; }

    }
}
