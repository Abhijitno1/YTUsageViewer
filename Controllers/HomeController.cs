using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Models;

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
            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection collection)
        {
            var searchCriteria = new CommentsSearch
            {
                Subscription = collection["cboSubscription"],
                Playlist = collection["txtPlaylist"],
                Channel = collection["txtChannel"],
                VideoName = collection["txtVideoName"],
                FromDate = Convert.ToDateTime(collection["dtFromDate"]),
                ToDate = Convert.ToDateTime(collection["dtToDate"]),
                CommentText = collection["txtCommentText"]
            };

            return View(searchCriteria);
        }

    }
}