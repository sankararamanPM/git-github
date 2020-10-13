using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using Equipment.Models;
using System.IO;
using System.Data.Entity;

namespace Equipment.Controllers
{
    public class CalibrationController : Controller
    {
        //
        // GET: /Calibration/
        EquipmentContext equipmentContext = new Models.EquipmentContext();
        public ActionResult Index(string Search)
        {
            //List<Calibration> lst = equipmentContext.Calibrations.ToList();
            //return View(lst);
            var lst = (from objcali in equipmentContext.Calibrations
                       join objequip in equipmentContext.Equipments on objcali.AssetReference equals objequip.Id
                       join objsept in equipmentContext.Departments on objequip.Department equals objsept.Id
                       select new CalibrationWithEquipment { calibration = objcali, equipment = objequip, department = objsept });
            if (!String.IsNullOrEmpty(Search))
            {
                lst = lst.Where(s => s.equipment.Asset.ToUpper().Contains(Search.ToUpper()) || s.department.Name.ToUpper().Contains(Search.ToUpper()));
            }
            var model = lst.ToList();
            return View(model);
        }
        public ActionResult Details(int id)
        {
           
            Calibration obj = new Calibration();
            obj = equipmentContext.Calibrations.Single(objid => objid.Id == id);
            obj.Department = equipmentContext.Equipments.FirstOrDefault(x => x.Id == obj.AssetReference).Department;
            GetDropwnListEdit(obj.AssetReference);
            JsonResult jsn = Checkphotoexist(obj.AssetReference);
            if (jsn.Data!=null && jsn.Data.ToString().Length > 0)
            {
                ViewBag.Photo = "Yes";
                ViewBag.PhotoData = jsn.Data;
            }
            else
            {
                ViewBag.Photo = "No";

            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
          
            Calibration obj = new Calibration();
            ViewBag.UserObj = obj;

            obj = equipmentContext.Calibrations.Single(objid => objid.Id == id);
            obj.Department = equipmentContext.Equipments.FirstOrDefault(x => x.Id == obj.AssetReference).Department;
            GetDropwnListEdit(obj.AssetReference);
            JsonResult jsn = Checkphotoexist(obj.AssetReference);


            if (jsn.Data != null && jsn.Data.ToString().Length > 0)
            {
                ViewBag.Photo = "Yes";
                ViewBag.PhotoData = jsn.Data;
            }
            else
            {
                ViewBag.Photo = "No";

            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Calibration calibration)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase filephoto = Request.Files[0];
                if (filephoto != null)
                {
                    Stream fs = filephoto.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    calibration.CalibReportpdf = bytes;
                }
                

                    //SendMail(Calibration.HandedOverToEmail);
                    //Calibration.MailStatus = TempData["Mailstatus"].ToString();
                    equipmentContext.Entry(calibration).State = EntityState.Modified;
                if (calibration.CalibReportpdf == null)
                {
                    equipmentContext.Entry(calibration).Property(x => x.CalibReportpdf).IsModified = false;
                }
                equipmentContext.SaveChanges();
                // SendMail(handover.HandedOverToEmail);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            
            Calibration obj = new Calibration();
            obj = equipmentContext.Calibrations.Single(objid => objid.Id == id);
            obj.Department = equipmentContext.Equipments.FirstOrDefault(x => x.Id == obj.AssetReference).Department;
            GetDropwnListEdit(obj.AssetReference);
            JsonResult jsn = Checkphotoexist(obj.AssetReference);
            if (jsn.Data != null && jsn.Data.ToString().Length > 0)
            {
                ViewBag.Photo = "Yes";
                ViewBag.PhotoData = jsn.Data;
            }
            else
            {
                ViewBag.Photo = "No";

            }
            return View(obj);



        }
        [HttpPost]
        public ActionResult Create(Calibration calibration, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // SendMail(calibration.HandedOverToEmail);
                calibration.AssetReference = Convert.ToInt32(TempData["ID"]);
                //calibration.MailStatus = TempData["Mailstatus"].ToString();

                if (file != null)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(file.InputStream))
                    {
                        bytes = br.ReadBytes(file.ContentLength);
                    }
                    calibration.CalibReportpdf = bytes;
                }
                equipmentContext.Calibrations.Add(calibration);
                equipmentContext.SaveChanges();

                return RedirectToAction("Index");

            }
            GetDropwnList();
            return View();
        }


        private ActionResult SendMail(string handover, string recivedto)
        {

            var settings = equipmentContext.settings.ToList();

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(settings.FirstOrDefault(x => x.KeyName == "SourceEmail").Value);
                mail.To.Add(handover);
                mail.CC.Add(recivedto);
                mail.Subject = "Hello World";
                mail.Body = "<h1>Hello</h1>";
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(settings.FirstOrDefault(x => x.KeyName == "SourceEmail").Value, settings.FirstOrDefault(x => x.KeyName == "SourcePWD").Value);
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                        TempData["Mailstatus"] = "Deleiverd";
                    }
                    catch (Exception)
                    {
                        TempData["Mailstatus"] = "Not Deleiverd";

                    }

                }
            }

            return null;

        }
        public ActionResult Create()
        {
            GetDropwnList();
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var calibration = equipmentContext.Calibrations.SingleOrDefault(x => x.Id == id);
            equipmentContext.Calibrations.Remove(calibration);
            equipmentContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DownloaddatapdfFile(int id)
        {

            Models.Calibration obj = equipmentContext.Calibrations.SingleOrDefault(x => x.Id == id);
            byte[] fileBytes = obj.CalibReportpdf;
            string fileName = "Calibration" + ".pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
        public ActionResult GetDropwnList()
        {
            ViewBag.Assetlist = null;
            ViewBag.Department = new SelectList(equipmentContext.Departments.ToList(), "Id", "Name");
            return null;
        }

        [HttpPost]
        public JsonResult GetAsset(int id)
        {
            var response = new SelectList(equipmentContext.Equipments.Where(x => x.Department == id).ToList(), "Id", "Asset");
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDropwnListEdit(int? id)
        {
            ViewBag.Assetlist = new SelectList(equipmentContext.Equipments.Where(x => x.Id == id).ToList(), "Id", "Asset");

            ViewBag.Department = new SelectList(equipmentContext.Departments.ToList(), "Id", "Name", 3);

            return null;
        }
        [HttpPost]
        public JsonResult Checkphotoexist(int id)
        {
            TempData["ID"] = id;
            Calibration obj = new Calibration();
            obj.equipment = equipmentContext.Equipments.FirstOrDefault(x => x.Id == id);
            byte[] photo = null;
            if (obj.equipment.Photo.Length > 0 && obj.equipment.Photo != null)
            {
                          photo = obj.equipment.Photo;
            }

            return Json(photo);
        }
    }
}
