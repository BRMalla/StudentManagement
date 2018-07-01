using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagement.Models;
using System.IO;

namespace StudentManagement.Controllers
{
    public class Frontend_messageController : Controller
    {
        private SMSEntities db = new SMSEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Frontend_message.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
			try
			{
				Frontend_message fm = new Frontend_message();
				fm = db.Frontend_message.Where(x => x.MessageId == id).FirstOrDefault();
				if (fm == null)
				{
					return HttpNotFound();
				}
				return View(fm);
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Frontend_message model)
        {
			try
			{
				if (ModelState.IsValid)
				{
					Frontend_message fc = new Frontend_message();
					fc.Message_description = model.Message_description;
					string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
					string extension = Path.GetExtension(model.ImageFile.FileName);
					filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
					model.Message_Image_Path = "~/Photo/Message/" + filename;
					filename = Path.Combine(Server.MapPath("~/Photo/Message/"), filename);
					db.Frontend_message.Add(model);
					model.ImageFile.SaveAs(filename);
					db.Frontend_message.Add(model);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				else
				{
					return HttpNotFound();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
			Frontend_message fm = new Frontend_message();
			fm = db.Frontend_message.Where(x => x.MessageId == id).FirstOrDefault();
			if (fm == null)
			{
				return HttpNotFound();
			}
			return View(fm);
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Frontend_message model)
        {
			if (ModelState.IsValid)
			{
				db.Entry(model).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				return HttpNotFound();
			}
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
			Frontend_message fm = new Frontend_message();
			fm = db.Frontend_message.Where(x => x.MessageId == id).FirstOrDefault();
			if (fm == null)
			{
				return HttpNotFound();
			}
			return View(fm);
		}


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Frontend_message frontend_message = db.Frontend_message.Find(id);
            db.Frontend_message.Remove(frontend_message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
