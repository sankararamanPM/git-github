using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Equipment.Models
{
     [Table("HandedOver")]
    public class HandOver
    {
       

        public int Id { get; set; }

        [Required]
        [DisplayName("Asset")]
        public int AssetReference { get; set; }

        
       [NotMapped]
        public byte[] Photo {get; set;}

        [Required]
        [DisplayName("Date")]
        public DateTime DateTime { get; set; }

        [Required]
        [DisplayName("Handed Over To")]
         public string HandedOverTo { get; set; }

        [Required]
        [DisplayName("Handed Over Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string HandedOverToEmail { get; set; }

        [Required]
        [DisplayName("Returned")]
        public String Returned { get; set; }

        [Required]
        [DisplayName("Recieved Date")]
        public DateTime RecivedDate { get; set; }

        [Required]
        [DisplayName("Recieved Email To")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String RecivedTo { get; set; }
       
        public String MailStatus { get; set; }
         [NotMapped]
       public  Equipment equipment { get; set; }

         [NotMapped]
         public int Department { get; set; }
        
    }
}