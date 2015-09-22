using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PWEapp.Models
{   
    [Table("pwe_reports")]
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}