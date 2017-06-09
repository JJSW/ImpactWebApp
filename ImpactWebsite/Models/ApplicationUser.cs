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
        [StringLength(160, MinimumLength = 1)]
        public string FirstName { get; set; }

        [StringLength(160, MinimumLength = 1)]
        public string LastName { get; set; }

        [StringLength(160, MinimumLength = 1)]
        public string CompanyName { get; set; }

        public bool NewsletterRequired { get; set; }

        public bool IsTempUser { get; set; }

        public List<Order> Orders { get; set; }

        public BillingAddress BillingAddress { get; set; }
    }
}
