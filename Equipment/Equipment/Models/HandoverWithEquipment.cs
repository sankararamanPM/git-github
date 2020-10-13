using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Equipment.Models
{
    public class HandOverWithEquipment
    {
        public HandOverWithEquipment()  
        {  
  
        }
        public HandOverWithEquipment(HandOver p, Equipment c)  
        {  
            //TO DO: Complete Member Initialization  
            this.handOver = p;
            this.equipment = c;  
        }
        public HandOver handOver { get; set; }
        public Equipment equipment { get; set; }
        public Department department { get; set; }  
    }
}