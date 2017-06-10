using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }

        public int OrderDetailId { get; set; }

        public int ModuleId { get; set; }

        public Module Module { get; set; }


    }
}
