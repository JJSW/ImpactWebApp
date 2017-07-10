using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class Module : BaseEntity
    {
        [Key]
        public Int32 ModuleId { get; set; }

        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        [Display(Name = "Module Description")]
        public string Description { get; set; }

        [Display(Name = "Module Description")]
        public string LongDescription { get; set; }

        public Int32 UnitPriceId { get; set; }

        [Display(Name = "Unit Price")]
        public UnitPrice UnitPrice { get; set; }

    }
}
