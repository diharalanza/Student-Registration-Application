//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcEfModelFirstStudentProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Student
    {
        public int stu_id { get; set; }
        [Required]
        public string full_name { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Must be a number with 10 digits.")]
        public string phone_number { get; set; }
        [Required]
        public string stu_address { get; set; }
        [Required]
        public System.DateTime birthday { get; set; }
        [Required]
        public System.DateTime join_date { get; set; }
        [Required]
        public float gpa { get; set; }
    }
}
