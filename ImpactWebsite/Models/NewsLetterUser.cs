using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models
{
    public class NewsLetterUser : BaseEntity
    {
        [Key]
        public Int64 NewsLetterUserId { get; set; }
        
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public bool isSubscribed { get; set; } = true;
    }
}
