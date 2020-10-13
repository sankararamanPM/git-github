using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Equipment.Models;

namespace Equipment.Controllers
{
    public class EquipmentController : Controller
    {
        //
        // GET: /Equipment/
        EquipmentContext equipmentContext = new Models.EquipmentContext();
        public ActionResult Index(string Search)
        {
            var lst = (from  objequip in equipmentContext.Equipments 
                       join objsept in equipmentContext.Departments on objequip.Department equals objsept.Id
                       select new EquipmentWithDepartment { equipment = objequip, department = objsept });
        //    List<Models.Equipment> lst = equipmentContext.Equipments.ToList();
            if (!String.IsNullOrEmpty(Search))
            {
                lst = lst.Where(s => s.equipment.Asset.ToUpper().Contains(Search.ToUpper()) || s.equipment.Category.ToUpper().Contains(Search.ToUpper()) || s.equipment.Brand.ToUpper().Contains(Search.ToUpper()) || s.equipment.PONo.ToUpper().Contains(Search.ToUpper()) || s.equipment.HandedOverto.ToUpper().Contains(Search.ToUpper()) || s.department.Name.ToUpper().Contains(Search.ToUpper()));
            }
            var model = lst.ToList();
            return View(model);  
        }

        
        public ActionResult Create()
        {
            GetDropwnList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.Equipment equipment)
        {
           
            if (ModelState.IsValid)
            {

                HttpPostedFileBase filephoto = Request.Files[0];
                HttpPostedFileBase fileOperatingInstructionpdf = Request.Files[1];
                HttpPostedFileBase fileDatasheetpdf = Request.Files[2];
                HttpPostedFileBase fileCataloguepdf = Request.Files[3];
                if (filephoto !=null)
                {
                    Stream fs = filephoto.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    equipment.Photo = bytes;
                }
                if (fileOperatingInstructionpdf != null)
                {
                    Stream fs = fileOperatingInstructionpdf.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    equipment.OperatingInstructionpdf = bytes;
                }
                if (fileDatasheetpdf != null)
                {
                    Stream fs = fileDatasheetpdf.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    equipment.Datasheetpdf = bytes;
                }
                if (fileCataloguepdf != null)
                {
                    Stream fs = fileCataloguepdf.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    equipment.Cataloguepdf = bytes;
                }
                equipmentContext.Equipments.Add(equipment);
                equipmentContext.SaveChanges();
                 return RedirectToAction("Index");

            }
            GetDropwnList();

            return View();
        }
        public ActionResult GetDropwnList()
        {
            ViewBag.FactoryUnitlist = new SelectList(equipmentContext.FactoryUnits.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(equipmentContext.Departments.ToList(), "Id", "Name");
            return null;
        }

        public ActionResult Details(int id)
        {
            GetDropwnList();
            Models.Equipment obj = new Models.Equipment();
            obj = equipmentContext.Equipments.Single(objid => objid.Id == id);
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            GetDropwnList();
            Models.Equipment obj = new Models.Equipment();
            ViewBag.UserObj = obj;
            obj = equipmentContext.Equipments.Single(objid => objid.Id == id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Edit(Models.Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                //Models.Equipment obj = new Models.Equipment();
                //obj = equipmentContext.Equipments.Single(objid => objid.Id == equipment.Id);
                HttpPostedFileBase filephoto = Request.Files[0];
                HttpPostedFileBase fileOperatingInstructionpdf = Request.Files[1];
                HttpPostedFileBase fileDatasheetpdf = Request.Files[2];
                HttpPostedFileBase fileCataloguepdf = Request.Files[3];
                if (filephoto.FileName != "")
                {
                    
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(filephoto.InputStream))
                    {
                        bytes = br.ReadBytes(filephoto.ContentLength);
                    }
                    equipment.Photo = bytes;
                }
               
                if (fileOperatingInstructionpdf.FileName != "")
                {
                    Stream fs = fileOperatingInstructionpdf.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    equipment.OperatingInstructionpdf = bytes;
                }
               
                if (fileDatasheetpdf.FileName != "")
                {
                    Stream fs = fileDatasheetpdf.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    equipment.Datasheetpdf = bytes;
                }
               
                if (fileCataloguepdf.FileName != "")
                {
                    Stream fs = fileCataloguepdf.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    equipment.Cataloguepdf = bytes;
                }
               
                equipmentContext.Entry(equipment).State = EntityState.Modified;
                if (equipment.Photo==null)
                {
                    equipmentContext.Entry(equipment).Property(x => x.Photo).IsModified = false;
                }
                if (equipment.OperatingInstructionpdf == null)
                {
                    equipmentContext.Entry(equipment).Property(x => x.OperatingInstructionpdf).IsModified = false;
                }
                if (equipment.Cataloguepdf == null)
                {
                    equipmentContext.Entry(equipment).Property(x => x.Cataloguepdf).IsModified = false;
                }
                if (equipment.Datasheetpdf == null)
                {
                    equipmentContext.Entry(equipment).Property(x => x.Datasheetpdf).IsModified = false;
                }
                equipmentContext.SaveChanges();
              
                return RedirectToAction("Index");
            }
            GetDropwnList();
            return View();
        }
        public ActionResult Delete(int? id)
        {
            GetDropwnList();
            Models.Equipment obj = new Models.Equipment();
            obj = equipmentContext.Equipments.Single(objid => objid.Id == id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var handover = equipmentContext.Equipments.SingleOrDefault(x => x.Id == id);
            equipmentContext.Equipments.Remove(handover);
            equipmentContext.SaveChanges();
            GetDropwnList();
            return RedirectToAction("Index");
        }
       
        public ActionResult DownloadoprpdfFile(int id)
        {

            Models.Equipment obj = equipmentContext.Equipments.SingleOrDefault(x => x.Id == id);
            byte[] fileBytes = obj.OperatingInstructionpdf;
            string fileName = obj.Asset+"Operationinstruction" + ".pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
        public ActionResult DownloaddatapdfFile(int id)
        {
            
            Models.Equipment obj = equipmentContext.Equipments.SingleOrDefault(x=>x.Id==id);
            byte[] fileBytes = obj.Datasheetpdf;
            string fileName = obj.Asset +"_datasheet" +".pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
        public ActionResult DownloadcatpdfFile(int id)
        {

            Models.Equipment obj = equipmentContext.Equipments.SingleOrDefault(x => x.Id == id);
            byte[] fileBytes = obj.Cataloguepdf;
            string fileName = obj.Asset + "_Catalogue" + ".pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
       
    }
}
