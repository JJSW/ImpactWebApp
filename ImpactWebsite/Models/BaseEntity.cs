﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactWebsite.Models
{
    public class BaseEntity
    {
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
