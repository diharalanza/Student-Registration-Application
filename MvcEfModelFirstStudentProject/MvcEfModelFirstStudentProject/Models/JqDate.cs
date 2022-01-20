using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StudentProject.Models
{
    public class JqDate
    {
        [Required]
        [Display(Name = "From: ")]
        public DateTime JoinDate1 { get; set; }
        [Required]
        [Display(Name = "To: ")]
        public DateTime JoinDate2 { get; set; }
    }
}