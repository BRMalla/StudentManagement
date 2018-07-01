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
using System.Data.SqlClient;

namespace StudentManagement.Controllers
{
	[Authorize(Roles = "Exam")]
	public class StudentDetailsController : Controller
    {
        private SMSEntities db = new SMSEntities();
        [HttpGet]
        public ActionResult Index(string faculty,string search)
        {
			var std = db.StudentDetails.Include(X => X.Faculty);
			if (!String.IsNullOrEmpty(search))
			{
				std = std.Where(x => x.StudentFirstName.Contains(search) || x.StudentLastName.Contains(search) || x.Faculty.FacultyName.Contains(search));
				ViewBag.Search = search;
			}

			var fc = std.OrderBy(x => x.Faculty.FacultyName).Select(x => x.Faculty.FacultyName).Distinct();
			if (!String.IsNullOrEmpty(faculty))
			{
				std = std.Where(x => x.Faculty.FacultyName == faculty);
			}
			ViewBag.fac = new SelectList(fc);
            return View(std.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
			StudentDetail sd = new StudentDetail();
			sd = db.StudentDetails.Where(x => x.StudentID == id).FirstOrDefault();
			if (sd == null)
			{
				return HttpNotFound();
			}
			return View(sd);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "BloodGroupId", "BloodGroupName");
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "GenderName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentDetail model)
        {
			try
			{
				if (ModelState.IsValid)
				{
					StudentDetail std = new StudentDetail();
					std.StudentFirstName = model.StudentFirstName;
					std.StudentLastName = model.StudentLastName;
					std.StudnetMiddleName = model.StudnetMiddleName;
					std.StudentParentFirstName = model.StudentParentFirstName;
					std.StudentParentMiddleName = model.StudentParentMiddleName;
					std.StudentParentLastName = model.StudentParentLastName;
					std.StudentMobileNo = model.StudentMobileNo;
					std.SrudentEmail = model.SrudentEmail;
					std.StudentParentMobileNo = model.StudentParentMobileNo;
					std.StudentAddress = model.StudentAddress;
					std.StudentDateOfBirth = model.StudentDateOfBirth;
					std.StudentNationality = model.StudentNationality;
					std.StudentMaritialStatus = model.StudentMaritialStatus;
					std.StudentPhoneNo = model.StudentPhoneNo;
					string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
					string extension = Path.GetExtension(model.ImageFile.FileName);
					filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
					model.Student_photo = "~/Photo/Student_Photo/" + filename;
					filename = Path.Combine(Server.MapPath("~/Photo/Student_Photo/"), filename);
					model.ImageFile.SaveAs(filename);
					db.StudentDetails.Add(model);
					db.SaveChanges();

					Marksheet mk = new Marksheet();
					SqlParameter p1 = new SqlParameter("@studentId", model.StudentID);
					SqlParameter p2 = new SqlParameter("@facultyId", model.FacultyID);
					SqlParameter p3 = new SqlParameter("@first_name", model.StudentFirstName);
					SqlParameter p4 = new SqlParameter("@middle_name", model.StudnetMiddleName);
					SqlParameter p5 = new SqlParameter("@last_name", model.StudentLastName);
					db.Database.ExecuteSqlCommand("inputmarks @studentId,@facultyId,@first_name,@middle_name,@last_name", p1, p2, p3, p4, p5);
					return RedirectToAction("Index");
				}

				ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "BloodGroupId", "BloodGroupName", model.BloodGroupId);
				ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", model.FacultyID);
				ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "GenderName", model.GenderId);
				return RedirectToAction("Index");

			}
			catch (Exception ex)
			{
				throw ex;
			}


		}

 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetail studentDetail = db.StudentDetails.Find(id);
            if (studentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "BloodGroupId", "BloodGroupName", studentDetail.BloodGroupId);
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", studentDetail.FacultyID);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "GenderName", studentDetail.GenderId);
            return View(studentDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( StudentDetail studentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BloodGroupId = new SelectList(db.BloodGroups, "BloodGroupId", "BloodGroupName", studentDetail.BloodGroupId);
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", studentDetail.FacultyID);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "GenderName", studentDetail.GenderId);
            return View(studentDetail);
        }

		[HttpGet]
        public ActionResult Delete(int? id)
        {
			StudentDetail sd = new StudentDetail();
			sd = db.StudentDetails.Where(x => x.StudentID == id).FirstOrDefault();
			if (sd == null)
			{
				return HttpNotFound();
			}
			return View(sd);
		}


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentDetail studentDetail = db.StudentDetails.Find(id);
            db.StudentDetails.Remove(studentDetail);
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
