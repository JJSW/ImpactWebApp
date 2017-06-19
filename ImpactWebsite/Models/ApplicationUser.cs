using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ImpactWebsite.Models.OrderModels;
using ImpactWebsite.Models.BillingModels;

namespace ImpactWebsite.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [StringLength(160, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(160, MinimumLength = 1)]
        public string LastName { get; set; }

        [Display(Name = "Company Name")]
        [StringLength(160, MinimumLength = 1)]
        public string CompanyName { get; set; }

        [Display(Name = "Newsletter")]
        public bool NewsletterRequired { get; set; }

        [Display(Name = "Temp User")]
        public bool IsTempUser { get; set; }

        public List<Order> Orders { get; set; }

        public BillingAddress BillingAddress { get; set; }
    }
}
