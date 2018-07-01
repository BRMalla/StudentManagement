using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private SMSEntities db = new SMSEntities();

		[HttpGet]
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Gender);
            return View(employees.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
			try
			{
				Employee em = new Employee();
				em = db.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();
				if (em == null)
				{
					return HttpNotFound();
				}
				return View(em);
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        [HttpGet]
        public ActionResult Create()
        {
			List<Gender> listgender = db.Genders.ToList();
			ViewBag.ligender = new SelectList(listgender, "GenderId", "GenderName");
			return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Employee model)
        {
			try
			{
				if (ModelState.IsValid)
				{
					List<Gender> listgender = db.Genders.ToList();
					ViewBag.ligender = new SelectList(listgender, "GenderId", "GenderName");
					Employee emp = new Employee();
					emp.Employee_First_Name = model.Employee_First_Name;
					emp.Employee_Middle_Name = model.Employee_Middle_Name;
					emp.Employee_Last_Name = model.Employee_Last_Name;
					emp.Employee_Address = model.Employee_Address;
					emp.Employee_Date_of_Birth = model.Employee_Date_of_Birth;
					emp.Employee_Parment_Address = model.Employee_Parment_Address;
					emp.Employee_Mobile_No = model.Employee_Mobile_No;
					emp.Employee_Email = model.Employee_Email;
					emp.Employee_Parents_First_Name = model.Employee_Parents_First_Name;
					emp.Employee_Parents_Middle_Name = model.Employee_Parents_Middle_Name;
					emp.Employee_Parents_Last_Name = model.Employee_Parents_Last_Name;
					emp.Employee_Rank = model.Employee_Rank;
					string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
					string extension = Path.GetExtension(model.ImageFile.FileName);
					filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
					model.Employee_Photo = "~/Photo/Employee_Photo/" + filename;
					filename = Path.Combine(Server.MapPath("~/Photo/Employee_Photo/"), filename);
					model.ImageFile.SaveAs(filename);
					db.Employees.Add(model);
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
			List<Gender> listgender = db.Genders.ToList();
			ViewBag.ligender = new SelectList(listgender, "GenderId", "GenderName");
			Employee empedit = new Employee();
			empedit = db.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();
			if (empedit == null)
			{
				return HttpNotFound();
			}
            return View(empedit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Employee model)
        {
			try
			{
				if (ModelState.IsValid)
				{
					List<Gender> listgender = db.Genders.ToList();
					ViewBag.ligender = new SelectList(listgender, "GenderId", "GenderName");
					db.Entry(model).State = System.Data.Entity.EntityState.Modified;
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
			Employee em = new Employee();
			em = db.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();
			if (em == null)
			{
				return HttpNotFound();
			}
			return View(em);
		}

 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
