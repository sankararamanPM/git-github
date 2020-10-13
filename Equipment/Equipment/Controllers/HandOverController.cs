using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Equipment.Models;

namespace Equipment.Controllers
{
    public class HandOverController : Controller
    {
        //
        // GET: /HandOver/
        EquipmentContext equipmentContext = new Models.EquipmentContext();
        public ActionResult Index(string Search)
        {

            var lst = (from objhand in equipmentContext.Handovers
                       join objequip in equipmentContext.Equipments on objhand.AssetReference equals objequip.Id
                       join objsept in equipmentContext.Departments on objequip.Department equals objsept.Id
                       select new HandOverWithEquipment { handOver = objhand, equipment = objequip,department=objsept });
            if (!String.IsNullOrEmpty(Search))
            {
                lst = lst.Where(s => s.equipment.Asset.ToUpper().Contains(Search.ToUpper()) || s.department.Name.ToUpper().Contains(Search.ToUpper()) || s.handOver.Returned.ToUpper().Contains(Search.ToUpper()) || s.handOver.HandedOverTo.ToUpper().Contains(Search.ToUpper()));
            }
            var model = lst.ToList();
            return View(model);  
           
        }

        public ActionResult Details( int id)
        {
          
            HandOver obj = new HandOver();
           obj = equipmentContext.Handovers.Single(objid => objid.Id == id);
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

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            
            HandOver obj = new HandOver();
            ViewBag.UserObj = obj;
                      
            obj = equipmentContext.Handovers.Single(objid => objid.Id == id);
            obj.Department= equipmentContext.Equipments.FirstOrDefault(x => x.Id == obj.AssetReference).Department;
            GetDropwnListEdit(obj.AssetReference);
            JsonResult jsn = Checkphotoexist(obj.AssetReference);
            if (jsn.Data != null &&  jsn.Data.ToString().Length > 0)
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
        public ActionResult Edit(HandOver  handover,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
               
                if (file != null)
                {
                    Stream fs = file.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    handover.Photo = bytes;
                }
                SendMail(handover.HandedOverToEmail,handover.RecivedTo);
                handover.MailStatus = TempData["Mailstatus"].ToString();
               
                               equipmentContext.Entry(handover).State = EntityState.Modified;
                equipmentContext.SaveChanges();
               // SendMail(handover.HandedOverToEmail);
                return RedirectToAction("Index");
            }
            GetDropwnList();
            return View();
        }
      
        public ActionResult Delete(int? id)
        {
           
            HandOver obj = new HandOver();
            obj = equipmentContext.Handovers.Single(objid => objid.Id == id);
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
        public ActionResult Create(HandOver handover)
        {
            if (ModelState.IsValid)
            {
                SendMail(handover.HandedOverToEmail,handover.RecivedTo);
                handover.AssetReference =Convert.ToInt32( TempData["ID"]);
                handover.MailStatus = TempData["Mailstatus"].ToString();
                equipmentContext.Handovers.Add(handover);   
                equipmentContext.SaveChanges();
               
                return RedirectToAction("Index");

            }
            
            return View();
        }


        private ActionResult SendMail(string handover,string recivedto)
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
                    catch (Exception ex)
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
            var handover = equipmentContext.Handovers.SingleOrDefault(x => x.Id == id);
            equipmentContext.Handovers.Remove(handover);
            equipmentContext.SaveChanges();
            return RedirectToAction("Index");
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
            ViewBag.Assetlist = new SelectList(equipmentContext.Equipments.Where(x=>x.Id==id).ToList(), "Id", "Asset"); 

            ViewBag.Department = new SelectList(equipmentContext.Departments.ToList(), "Id", "Name",3);
            
            return null;
        }

        [HttpPost]
        public JsonResult Checkphotoexist(int id)
        {
            TempData["ID"] = id;
            byte[] photo = equipmentContext.Equipments.FirstOrDefault(x => x.Id == id).Photo;
            byte[] response  = null;
            if (photo.Length > 0 && photo != null)
            {
                response = photo;
            }

            return Json(response);
        }
    }
}
