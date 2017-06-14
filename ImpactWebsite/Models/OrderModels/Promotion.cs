using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class Promotion : BaseEntity
    {
        [Key]
        public int PromotionId { get; set; }

        [StringLength(160, MinimumLength = 2)]
        [Display(Name = "Promotion Name")]
        public string PromotionName { get; set; }

        [Required]
        [StringLength(8, ErrorMessage="Please input {1} letters", MinimumLength = 8)]
        [Display(Name = "Promotion Code")]
        public string PromotionCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P2}")]
        [Range(0.0, double.MaxValue)]
        [Display(Name = "Discount Rate")]
        public decimal DiscountRate { get; set; }

        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date from")]
        public DateTime DateFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date to")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
