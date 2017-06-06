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
        public string PromotionName { get; set; }

        [Required]
        [StringLength(8, ErrorMessage="Please input {1} letters", MinimumLength = 8)]
        public string PromotionCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P2}")]
        [Range(0.0, double.MaxValue)]
        public Decimal DiscountRate { get; set; }
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
