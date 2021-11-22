using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Models;

namespace YTUsageViewer.Controllers
{
    public class CommentsController : Controller
    {
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            DateTime fromDate, toDate;

            var searchCriteria = new CommentsSearch
            {
                Subscription = collection["cboSubscription"],
                Playlist = collection["txtPlaylist"],
                Channel = collection["txtChannel"],
                VideoName = collection["txtVideoName"],
                CommentText = collection["txtCommentText"]
            };
            if (!string.IsNullOrWhiteSpace(collection["dtFromDate"]))
                if (DateTime.TryParse(collection["dtFromDate"], out fromDate))
                    searchCriteria.FromDate = fromDate;

            if (!string.IsNullOrWhiteSpace(collection["dtToDate"]))
                if (DateTime.TryParse(collection["dtToDate"], out toDate))
                    searchCriteria.ToDate = toDate;

            return PartialView("SearchResult", searchCriteria);
        }
    }
}