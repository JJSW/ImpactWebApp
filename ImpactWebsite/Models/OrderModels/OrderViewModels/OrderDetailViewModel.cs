using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models.OrderModels
{
    public class OrderDetailViewModel
    {
        [Display(Name = "Order ID")]
        public Int32 OrderId { get; set; }

        [Display(Name = "Order Number")]
        public string OrderNum { get; set; }

        [Display(Name = "Order Detail Number")]
        public Int32 OrderDetailId { get; set; }

        [Display(Name = "Module ID")]
        public Int32 ModuleId { get; set; }

        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Module Price")]
        public int UnitPrice { get; set; }

        [Display(Name = "Total Amount")]
        public int TotalAmount { get; set; }

        [Display(Name = "Note")]
        public string NoteFromUser { get; set; }

        [Display(Name = "Note From Admin")]
        public string NoteFromAdmin { get; set; }

        [Display(Name = "Uploaded File Path")]
        public string UploadedFilePath { get; set; }

        [Display(Name = "Uploaded File Name")]
        public string UploadedFileName { get; set; }

        [Display(Name = "Billing Address Number")]
        public Int32 BillingAddressId { get; set; }

        [Display(Name = "Promotion")]
        public Promotion Promotion { get; set; }

    }
}
