using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Equipment.Models
{
    [Table("Settings")]
    public class Settings
    {
        
        public int Id { get; set; }
        public String KeyName { get; set; }
        public String Value { get; set; }
    }
}