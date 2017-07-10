using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models
{
    public class UnitPrice : BaseEntity
    {
        [Key]
        public Int32 UnitPriceId { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Display(Name = "Date Effect From")]
        public DateTime DateEffectFrom { get; set; }

        [Display(Name = "Date Effect To")]
        public DateTime DateEffectTo { get; set; }
    }
}