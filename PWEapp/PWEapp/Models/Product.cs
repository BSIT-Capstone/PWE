using System.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace PWEapp.Models
{
    [Table("pwe_products")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
 
        public int LocaleID { get; set; }
       
        public int PriceGroupID { get; set; }
       
        public string WebChg { get; set; }
    }

  
}