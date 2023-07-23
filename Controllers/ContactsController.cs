using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Helpers;
using YTUsageViewer.Models;

namespace YTUsageViewer.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contacts
        public ActionResult Index(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            const int PAGE_SIZE = 10;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetSearchResults(searchString, sortOrder, sortDir);
            return View(result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));
        }

        private IQueryable<Contact> GetSearchResults(string searchString, string sortOrder, string sortDir)
        {
            var result = db.Contacts.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                result = result.Where(x => (x.FirstName != null && x.FirstName.ToLower().Contains(searchString.ToLower()))
                    || (x.LastName != null && x.LastName.ToLower().Contains(searchString.ToLower())));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortDir = sortDir;

                if (sortOrder == "firstName" && sortDir == "ASC")
                    result = result.OrderBy(x => x.FirstName);
                else if (sortOrder == "firstName" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.FirstName);
                else if (sortOrder == "lastName" && sortDir == "ASC")
                    result = result.OrderBy(x => x.LastName);
                if (sortOrder == "lastName" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.LastName);
                if (sortOrder == "email" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Email);
                else if (sortOrder == "email" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Email);
            }
            else
                result = result.OrderBy(x => x.ID);

            return result;
        }

        public ActionResult Export2SpreadsheetML(string searchString, string sortOrder, string sortDir)
        {
            try
            {
                var result = GetSearchResults(searchString, sortOrder, sortDir);
                IExcelExporter xlXporter = new ExcelExporterWithXsl();
                var outputStream = xlXporter.ExportDataAsSpreadsheet(result);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string attachmentName = "Contacts-xls.xml";
                return File(outputStream, contentType, attachmentName);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportTabDelimted(string searchString, string sortOrder, string sortDir)
        {
            var result = GetSearchResults(searchString, sortOrder, sortDir);
            ExcelExporterWithXsl xlXporter = new ExcelExporterWithXsl();
            var outputStream = xlXporter.ExportContactsTabDelimited(result);

            string contentType = "application/vnd.ms-excel";
            string attachmentName = "Contacts.xls";
            return File(outputStream, contentType, attachmentName);
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,PhoneMobile,PhoneWork,PhoneHome,PreferredPhone,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,PhoneMobile,PhoneWork,PhoneHome,PreferredPhone,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
