using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models
{
    public class Investment : BaseEntity
    {
        [Key]
        public Int64 InvestmentId { get; set; }

        [Required]
        [StringLength(160, MinimumLength = 2)]
        public string InvestmentName { get; set; }

        [StringLength(160, MinimumLength = 1)]
        public string ISIN { get; set; }

        public int ShareNumber { get; set; }

        [Required]
        [Range(0.00, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal EstimateValue { get; set; }                
    }
}
