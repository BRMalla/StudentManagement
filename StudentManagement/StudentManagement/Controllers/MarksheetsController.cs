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
    public class MarksheetsController : Controller
    {
        private SMSEntities db = new SMSEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var marksheets = db.Marksheets.Include(m => m.Faculty).Include(m => m.StudentDetail);
            return View(marksheets.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
			try
			{
				Marksheet mk = new Marksheet();
				mk = db.Marksheets.Where(x => x.marksheetID == id).FirstOrDefault();
				if (mk == null)
				{
					return HttpNotFound();
				}
				return View(mk);
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            ViewBag.StudentID = new SelectList(db.StudentDetails, "StudentID", "StudentFirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Marksheet model)
        {
            if (ModelState.IsValid)
            {
                db.Marksheets.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", model.FacultyID);
            ViewBag.StudentID = new SelectList(db.StudentDetails, "StudentID", "StudentFirstName", model.StudentID);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marksheet marksheet = db.Marksheets.Find(id);
            if (marksheet == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marksheet.FacultyID);
            ViewBag.StudentID = new SelectList(db.StudentDetails, "StudentID", "StudentFirstName", marksheet.StudentID);
            return View(marksheet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Marksheet model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", model.FacultyID);
            ViewBag.StudentID = new SelectList(db.StudentDetails, "StudentID", "StudentFirstName", model.StudentID);
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
			Marksheet mk = new Marksheet();
			mk = db.Marksheets.Where(x => x.marksheetID == id).FirstOrDefault();
			if (mk == null)
			{
				return HttpNotFound();
			}
			return View(mk);
		}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marksheet marksheet = db.Marksheets.Find(id);
            db.Marksheets.Remove(marksheet);
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
