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
	[Authorize(Roles = "Account")]
	public class AccountsController : Controller
    {
        private SMSEntities db = new SMSEntities();

		[HttpGet]
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }
		[HttpGet]
        public ActionResult Details(int? id)
        {
			try
			{
				Account acc = new Account();
				acc = db.Accounts.Where(x => x.AccountID == id).FirstOrDefault();
				if (acc == null)
				{
					return HttpNotFound();
				}
				return View(acc);
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
        public ActionResult Create( Account model)
        {
			if (ModelState.IsValid)
			{
				Account acc = new Account();
				acc.AccountUserName = model.AccountUserName;
				acc.Student_first_name = model.Student_first_name;
				acc.Student_middle_name = model.Student_middle_name;
				acc.Student_last_name = model.Student_last_name;
				acc.Employee_first_name = model.Employee_first_name;
				acc.Employee_middle_name = model.Employee_middle_name;
				acc.Employee_last_name = model.Employee_last_name;
				acc.Amount_Paid = model.Amount_Paid;
				acc.Date = model.Date;
				acc.Remark = model.Remark;
				db.Accounts.Add(model);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				return HttpNotFound();
			}
        }

        public ActionResult Edit(int? id)
        {
			Account acc = new Account();
			acc = db.Accounts.Where(x => x.AccountID == id).FirstOrDefault();
			if (acc == null)
			{
				return HttpNotFound();
			}
			return View(acc);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account model)
        {
			try
			{
				if (ModelState.IsValid)
				{
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


        public ActionResult Delete(int? id)
        {
			Account acc = new Account();
			acc = db.Accounts.Where(x => x.AccountID == id).FirstOrDefault();
			if (acc == null)
			{
				return HttpNotFound();
			}
			return View(acc);

		}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
