using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using YTUsageViewer.Models;

namespace YTUsageViewer.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationRoleManager roleManager;

        public RolesController()
        {

        }

        public RolesController(ApplicationRoleManager roleManager)
        {
            this.RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return this.roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                this.roleManager = value;
            }
        }

        // GET: Role
        public ActionResult Index()
        {
            //List<RoleViewModel> roles = RoleManager.Roles.Select(x => new RoleViewModel { Id = x.Id, Name = x.Name }).ToList();
            List<RoleViewModel> roles = new List<RoleViewModel>();
            foreach (var appRole in RoleManager.Roles)
            {
                roles.Add(new RoleViewModel { Id = appRole.Id, Name = appRole.Name });
            }

            return View(roles);
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                // TODO: Add validation for name
                var role = new ApplicationRole(collection["name"]);
                await RoleManager.CreateAsync(role);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }

        // POST: Role/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            try
            {
                // TODO: validate the incoming model here
                var role = new ApplicationRole() { Id = model.Id, Name = model.Name };
                await RoleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var role = await RoleManager.FindByIdAsync(id);
                await RoleManager.DeleteAsync(role);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
