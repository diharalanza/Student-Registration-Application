using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcEfCodeFirst.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base("CodeFirstTest")
        {
        }
            
        public DbSet<StudentModel> Students { get; set; }
    }

}