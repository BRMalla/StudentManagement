using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
	[Authorize(Roles = "Admin")]
	[RoutePrefix("NT")]
	public class NoticesController : Controller
    {
        private SMSEntities db = new SMSEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Notices.ToList());
        }

        [HttpGet]

        public ActionResult Details(int? id)
        {
			try
			{
				Notice nt = new Notice();
				nt = db.Notices.Where(x => x.NoticeId == id).FirstOrDefault();
				if (nt == null)
				{
					return HttpNotFound();
				}
				return View(nt);
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        [HttpGet]
		[Route("NTC")]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
		[Route("NTC")]
		[ValidateAntiForgeryToken]
        public ActionResult Create( Notice model)
        {
			try
			{
				if (ModelState.IsValid)
				{
					Notice nt = new Notice();
					nt.Subject = model.Subject;
					nt.Publish_Date = model.Publish_Date;
					nt.Description = model.Description;
					db.Notices.Add(model);
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
			Notice nt = new Notice();
			nt = db.Notices.Where(x => x.NoticeId == id).FirstOrDefault();
			return View(nt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Notice model)
        {
			try
			{
				if (ModelState.IsValid)
				{
					Notice nt = new Notice();
					db.Entry(model).State = EntityState.Modified;
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
        public ActionResult Delete(int? id)
        {
			Notice nt = new Notice();
			nt = db.Notices.Where(x => x.NoticeId == id).FirstOrDefault();
			if (nt == null)
			{
				return HttpNotFound();
			}
			return View(nt);
		}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = db.Notices.Find(id);
            db.Notices.Remove(notice);
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
