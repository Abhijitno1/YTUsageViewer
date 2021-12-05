using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Helpers;

namespace YTUsageViewer.Controllers
{
    public class BulkUploadController : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }

        //ToDo: Convert to async method
        public ActionResult UploadCSV(HttpPostedFileBase fileUpload)
        {
            try
            {
                if (fileUpload != null)
                {
                    //fileUpload.SaveAs(Path.Combine(Server.MapPath("~/Assets/"), fileUpload.FileName));
                    if (!fileUpload.FileName.ToLower().EndsWith(".csv"))
                    {
                        ModelState.AddModelError("fileUpload", "Please select a CSV file to upload");
                        return PartialView("Index");
                    }
                    CsvImporter importer = new CsvImporter();
                    DataTable newContacts = importer.GetDataTableFromCSVFile(fileUpload.InputStream);
                    importer.SaveContacts2DB(newContacts);
                    //return RedirectToAction("Index", "Movie");
                    var response = new { Success = true, Message = "Contacts uploaded successfully" };
                    return Json(response);
                }
                else
                {
                    ModelState.AddModelError("fileUpload", "Please select a file to upload");
                    return PartialView("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("fileUpload", ex);
                return PartialView("Index");
            }
        }
    }
}