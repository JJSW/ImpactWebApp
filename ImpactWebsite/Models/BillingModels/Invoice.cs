using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.BillingModels
{
    public class Invoice : BaseEntity
    {

        public int? AmountDue { get; set; }

        public int? ApplicationFee { get; set; }

        public int? AttemptCount { get; set; }

        public bool? Attempted { get; set; }

        public virtual BillingAddress BillingAddress { get; set; }

        public bool? Closed { get; set; }

        public string Currency { get; set; }

        [NotMapped]
        public Currency CurrencyDetails { get; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public int? EndingBalance { get; set; }

        public int InvoiceId { get; set; }

        [NotMapped]
        public string InvoicePeriod { get; }

       // public virtual ICollection<LineItem> LineItems { get; set; }

        public DateTime? NextPaymentAttempt { get; set; }

        public bool? Paid { get; set; }

        public string StatementDescriptor { get; set; }

        [MaxLength(50)]
        public string StripeCustomerId { get; set; }

        [MaxLength(50)]
        public string StripeId { get; set; }

        public int? Subtotal { get; set; }

        public int? Tax { get; set; }

        public decimal? TaxPercent { get; set; }

        public int? Total { get; set; }
    }
}
