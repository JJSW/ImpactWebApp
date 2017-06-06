using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class UnitPrice
    {
        [Key]
        public int UnitPriceId { get; set; }
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        public DateTime DateEffectFrom { get; set; }
        public DateTime DateEffectTo { get; set; }
    }
}