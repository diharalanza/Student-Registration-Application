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

        public DateTime JoinDate1 { get; set; }
        public DateTime JoinDate2 { get; set; }
    }
}