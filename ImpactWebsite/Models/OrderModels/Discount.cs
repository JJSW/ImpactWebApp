using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class Discount
    {
        [Key]
        public Int32 DiscountId { get; set; }
       
        [Display(Name = "Discount Name")]
        public string DiscountName { get; set; }

        [Display(Name = "Discount Rate")]
        public int DiscountRate { get; set; }

        [Display(Name = "Select From")]
        public int SelectFrom { get; set; }

        [Display(Name = "Select To")]
        public int SelectTo { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
