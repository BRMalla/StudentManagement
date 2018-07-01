using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using StudentManagement.Models;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersAdminController : Controller
    {
		public UsersAdminController()
		{
		}

		public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager
		 roleManager)
		{
			UserManager = userManager;
			RoleManager = roleManager;
		}

		private ApplicationUserManager _userManager;
		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ??
			 HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		private ApplicationRoleManager _roleManager;
		public ApplicationRoleManager RoleManager
		{
			get
			{
				return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
			}
			private set
			{
				_roleManager = value;
			}
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			return View(await UserManager.Users.ToListAsync());
		}

		[HttpGet]
		public async Task<ActionResult> Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var user = await UserManager.FindByIdAsync(id);

			ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

			return View(user);
		}

		[HttpGet]
		public async Task<ActionResult> Create()
		{
			//Get the list of Roles
			ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[]
			selectedRoles)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = userViewModel.Email,
					Email = userViewModel.Email,
					DateOfBirth = userViewModel.DateOfBirth,
					FirstName = userViewModel.FirstName,
					LastName = userViewModel.LastName,
					Address = userViewModel.Address
				};
				var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

				//Add User to the selected Roles 
				if (adminresult.Succeeded)
				{
					if (selectedRoles != null)
					{
						var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
						if (!result.Succeeded)
						{
							ModelState.AddModelError("", result.Errors.First());
							ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
							return View();
						}
					}
				}
				else
				{
					ModelState.AddModelError("", adminresult.Errors.First());
					ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
					return View();

				}
				return RedirectToAction("Index");
			}
			ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
			return View();
		}

		[HttpGet]
		public async Task<ActionResult> Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var user = await UserManager.FindByIdAsync(id);
			if (user == null)
			{
				return HttpNotFound();
			}

			var userRoles = await UserManager.GetRolesAsync(user.Id);

			return View(new EditUserViewModel()
			{
				Id = user.Id,
				Email = user.Email,
				DateOfBirth = user.DateOfBirth,
				FirstName = user.FirstName,
				LastName = user.LastName,
				RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
				{
					Selected = userRoles.Contains(x.Name),
					Text = x.Name,
					Value = x.Name
				})
			});
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditUserViewModel editUser, params string[] selectedRole)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByIdAsync(editUser.Id);
				if (user == null)
				{
					return HttpNotFound();
				}

				user.DateOfBirth = editUser.DateOfBirth;
				user.FirstName = editUser.FirstName;
				user.LastName = editUser.LastName;

				var userRoles = await UserManager.GetRolesAsync(user.Id);

				selectedRole = selectedRole ?? new string[] { };

				var result = await UserManager.AddToRolesAsync(user.Id,
				 selectedRole.Except(userRoles).ToArray<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First());
					return View();
				}
				result = await UserManager.RemoveFromRolesAsync(user.Id,
				 userRoles.Except(selectedRole).ToArray<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First());
					return View();
				}
				return RedirectToAction("Index");
			}
			ModelState.AddModelError("", "Something failed.");
			return View();
		}		
       
    }
}
