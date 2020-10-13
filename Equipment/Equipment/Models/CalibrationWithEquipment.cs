using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Equipment.Models
{
    public class CalibrationWithEquipment
    {
        public CalibrationWithEquipment()  
        {  
  
        }
        public CalibrationWithEquipment(Calibration p, Equipment c,Department department)  
        {  
            //TO DO: Complete Member Initialization  
            this.calibration = p;
            this.equipment = c;
            this.department = department;
        }
        public Calibration calibration { get; set; }
        public Equipment equipment { get; set; }
        public Department department { get; set; }  
    }
}