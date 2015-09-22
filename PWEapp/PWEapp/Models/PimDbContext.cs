using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PWEapp.Models
{
    public class PimDbContext: DbContext
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}