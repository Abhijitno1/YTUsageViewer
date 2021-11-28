using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using YTUsageViewer.Models;

namespace YTUsageViewer.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UsersController()
        {

        }

        public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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

        // GET: Users
        public ActionResult Index()
        {
            List<UserViewModel> appUsers = new List<UserViewModel>();
            foreach (var x in UserManager.Users)
            {
                appUsers.Add(new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    EmailConfirmed = x.EmailConfirmed,
                    PhoneNumber = x.PhoneNumber,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                    LockoutEnabled = x.LockoutEnabled,
                    LockoutEndDate = x.LockoutEndDateUtc
                });
            }
            return View(appUsers);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            var x = UserManager.FindById(id);
            var appUser = new UserViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumber = x.PhoneNumber,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                LockoutEnabled = x.LockoutEnabled,
                LockoutEndDate = x.LockoutEndDateUtc
            };

            var userRoles = UserManager.GetRoles(id);
            var roles = userRoles.Select(y => new UserRoleViewModel { RoleName = y });
            appUser.Roles = roles.ToList();

            return View(appUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            var dbUser = UserManager.FindById(id);
            var appUser = BuildUserViewModel(dbUser);
            return View(appUser);
        }

        private UserViewModel BuildUserViewModel(ApplicationUser dbUser)
        {
            var dbUserRoles = dbUser.Roles.ToList();
            var appUser = new UserViewModel
            {
                Id = dbUser.Id,
                UserName = dbUser.UserName,
                Email = dbUser.Email,
                PhoneNumber = dbUser.PhoneNumber
            };
            var userRoles = RoleManager.Roles.Select(y => new UserRoleViewModel
            {
                RoleId = y.Id,
                RoleName = y.Name
            }).ToList();
            userRoles.ForEach(y => y.IsSelected = dbUserRoles.Any(z => z.RoleId == y.RoleId));
            appUser.Roles = userRoles;
            return appUser;
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(string Id, FormCollection collection)
        {
            var dbUser = UserManager.FindById(Id);
            dbUser.UserName = collection["UserName"];
            dbUser.Email = collection["Email"];
            dbUser.PhoneNumber = collection["PhoneNumber"];
            var result = UserManager.Update(dbUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("UserUpdateError", error);
                }
            }
            // Now add/remove roles for user
            var existingRoles = UserManager.GetRoles(dbUser.Id);
            var selectedRoles = collection["chkIsSelected"].Split(',');
            var addedRoles = selectedRoles.Except(existingRoles).ToList();
            var removedRoles = existingRoles.Except(selectedRoles).ToList();

            foreach (var role in addedRoles)
            {
                result = UserManager.AddToRole(dbUser.Id, role);
            }
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("AddRoleError", error);
                }
            }
            foreach (var role in removedRoles)
            {
                result = UserManager.RemoveFromRole(dbUser.Id, role);
            }
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RemoveRoleError", error);
                }
            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");
            else
            {
                var appUser = BuildUserViewModel(dbUser);
                return View(appUser);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

    }
}
