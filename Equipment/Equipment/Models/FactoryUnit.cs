using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
namespace Equipment.Models
{
    [Table("FactoryUnit")]
    public class FactoryUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public SelectList FactoryUnitlist { get; set; } 
    }
}