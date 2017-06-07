using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public enum OrderStatusList
    {
        AwaitingPayment = 0,
        Processing = 1,
        Completed = 2,
        Cancelled = 3,       
    }

    public class Order
    {
        [Key]
        [Display(Name = "Order Number")]
        public int OrderId { get; set; }

        public int OrderNum { get; set; }

        [Display(Name = "Sales Representative Info")]
        [StringLength(160, MinimumLength = 2)]
        public string SalesRep { get; set; }

        [Display(Name = "Ordered Date")]
        public DateTime OrderedDate{ get; set; }

        [Display(Name = "Delivered Date")]
        public DateTime DeliveredDate { get; set; }

        // Uses list of order statuses
        [Display(Name = "Order Status")]
        public OrderStatusList OrderStatus { get; set; }

        [Display(Name = "Note")]
        public string NoteFromUser { get; set; }
        public string NoteFromAdmin { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        public string UserId { get; set; }
        //public Promotion Promotion { get; set; }
        //public int PromotionId { get; set; }


        public List<OrderDetail> OrderDetails { get; set; }

        [Display(Name = "Total Amount")]
        public int TotalAmount { get; set; }
    
        //public List<Investment> Investments { get; set; }
    }
}
