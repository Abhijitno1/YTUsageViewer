using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Models;

namespace YTUsageViewer.Controllers
{
    public class YTBrowserController : Controller
    {
        // GET: Subscriptions
        public ActionResult Index()
        {
            var subscriptions = new List<Subscription>();
            return View("Subscriptions", subscriptions);
        }

        public ActionResult Playlists()
        {
            var playlists = new List<Playlist>();
            return View(playlists);
        }

        public ActionResult Channels()
        {
            var channels = new List<Channel>();
            return View(channels);
        }

        public ActionResult Videos()
        {
            var videos = new List<Video>();
            return View(videos);
        }

        // GET: Subscriptions/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
     
    }
}
