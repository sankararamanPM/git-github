using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Equipment.Models
{
    public class EquipmentWithDepartment
    {
         public EquipmentWithDepartment()  
        {  
  
        }
         public EquipmentWithDepartment(Department p, Equipment c)  
        {  
            //TO DO: Complete Member Initialization  
            this.department = p;
            this.equipment = c;  
        }
        
        public Equipment equipment { get; set; }
        public Department department { get; set; }  
    }
}