using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment.Models
{
    [Table("Equipment")]
    public class Equipment
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Asset")]
        public string Asset { get; set; }

        [Required]
        [DisplayName("Category")]
        public string Category { get; set; }

        [Required]
        [DisplayName("Specification")]
        public string Specification { get; set; }

        [Required]
        [DisplayName("Brand")]
        public string Brand { get; set; }

        [Required]
        [DisplayName("Purchased Vendor Details")]
        public string PurchasedVendorDetails { get; set; }

        [Required]
        [DisplayName("Calibration Vendor Details")]
        public string CalibrationVendorDetails { get; set; }

        [Required]
        [DisplayName("Purchased Vendor Email")]
        public string PurchasedVendorEmail { get; set; }

        [Required]
        [DisplayName("Calibration Vendor Email")]
        public string CalibrationVendorEmail { get; set; }

        [Required]
        [DisplayName("PONo")]
        public string PONo { get; set; }

        [Required]
        [DisplayName("PODate")]
        public DateTime PODate { get; set; }

        [Required]
        [DisplayName("RecievedDate")]
        public DateTime RecievedDate { get; set; }

        [Required]
        [DisplayName("Factoryunit")]
        public int Factoryunit { get; set; }

        [Required]
        [DisplayName("Department")]
        public int Department { get; set; }

      
        [DisplayName("Photo")]
        public byte[] Photo { get; set; }

        
        [DisplayName("Operating Instruction(pdf)")]
        public byte[] OperatingInstructionpdf { get; set; }

      
        [DisplayName("Datasheet(pdf)")]
        public byte[] Datasheetpdf { get; set; }

        [DisplayName("Catalogue(pdf)")]
        public byte[] Cataloguepdf { get; set; }

        [Required]
        [DisplayName("Responsible")]
        public string Responsible { get; set; }

        [Required]
        [DisplayName("EmailId")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Emailid { get; set; }

        [Required]
        [DisplayName("Empcd")]
        public string Empcd { get; set; }

        [Required]
        [DisplayName("Specification(256)")]
        public string Specification1 { get; set; }

        [Required]
        [DisplayName("Purchased Date")]
        public DateTime Purchaseddate { get; set; }

        [Required]
        [DisplayName("Value")]
        public decimal Value { get; set; }

        [Required]
        [DisplayName("Age")]
        public int Age { get; set; }

        [Required]
        [DisplayName("Prev calibration")]
        public string Prevcalibration { get; set; }

        [Required]
        [DisplayName("Calibration due date")]
        public DateTime Calibrationduedate { get; set; }

        [Required]
        [DisplayName("Status")]
        public string Active { get; set; }

        [Required]
        [DisplayName("Handed Over To")]
        public string HandedOverto { get; set; }

        [Required]
        [DisplayName("Remark1")]
        public string Remark1    { get; set; }

        [Required]
        [DisplayName("Remark2")]
        public string Remark2 { get; set; }

        [NotMapped]
        public FactoryUnit FactoryUnits;


        [NotMapped]
        public Department Departments;
    }
}