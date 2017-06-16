using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class TempOrder
    {
        public bool IsChecked { get; set; }

        public Module Modules { get; set; }
    }
}
