using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Helpers;
using YTUsageViewer.Models;
using YTUsageViewer.ViewModels;

namespace YTUsageViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.CurrentPractMenu = PracticePages.Contact;
            return View(new CommentsSearch());
        }

        public ActionResult InfiniteScroll() 
        {
          ViewBag.CurrentPractMenu = PracticePages.InfiniteScroll;
          return View();
        }

        public ActionResult SearchVideoDialog(SearchVideoParams srchParams)
        {
            return PartialView(srchParams);
        }

    }
}