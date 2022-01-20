using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcEfCodeFirst.Models
{
    public class StudentModel
    {
        
        [Required]
        [Key]
        public int stu_id { get; set; }
        [Required]
        public string full_name { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Must be a number with 10 digits.")]
        public string phone_number { get; set; }
        [Required]
        public string stu_address { get; set; }
        [Required]
        public DateTime birthday { get; set; }
        [Required]
        public DateTime join_date { get; set; }
        [Required]
        public float gpa { get; set; }
    }
}