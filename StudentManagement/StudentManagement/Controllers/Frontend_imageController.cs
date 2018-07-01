using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManagement.Models;
using System.IO;

namespace StudentManagement.Controllers
{
    public class Frontend_imageController : Controller
    {
		SMSEntities db = new SMSEntities();
		[HttpGet]
		public ActionResult Index()
        {
            return View();
        }
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}


		[HttpPost]
		public ActionResult Create(Frontend_Image model)
		{
			
				if (ModelState.IsValid)
				{
					string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
					string extension = Path.GetExtension(model.ImageFile.FileName);
					filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
					model.Image_Path = "~/Photo/Frontend_Photo/" + filename;
					filename = Path.Combine(Server.MapPath("~/Photo/Frontend_Photo/"), filename);
					model.ImageFile.SaveAs(filename);
					db.Frontend_Image.Add(model);
					db.SaveChanges();
					return View();

				}
				else
				{
					return HttpNotFound();
				}
			
			
		}

		[HttpGet]
		public ActionResult View(int id)
		{
			Frontend_Image vi = new Frontend_Image();
			vi = db.Frontend_Image.Where(x => x.ImageID == id).FirstOrDefault();
			return View(vi);
		}

	}
}