using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Equipment.Models
{
    [Table("Calibration")]
    public class Calibration
    {

        public int Id { get; set; }
        [Required]
        [DisplayName("Asset")]
        public int AssetReference { get; set; }

        [Required]
        [DisplayName("Calibration Date")]
        [DataType(DataType.Date)]
        
        public DateTime Calibdate { get; set; }

        [Required]
        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }

         [DisplayName("Calibration Report(pdf) ")]
        public byte[] CalibReportpdf { get; set; }


         [Required]
         public string Remarks { get; set; }
        
        
        public String MailStatus { get; set; }


        [NotMapped]
        public String Photo { get; set; }

        [NotMapped]
        public Equipment equipment { get; set; }


        [NotMapped]
        public int Department { get; set; }
    }
}