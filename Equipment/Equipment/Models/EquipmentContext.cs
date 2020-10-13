using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Equipment.Models
{
    public class EquipmentContext : DbContext
    {
        public DbSet<HandOver> Handovers { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<FactoryUnit> FactoryUnits { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Calibration> Calibrations { get; set; }
        public DbSet<Settings> settings { get; set; }
    }
}