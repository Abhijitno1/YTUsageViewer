using PagedList;
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
        private ApplicationDbContext db = new ApplicationDbContext();
        private const int PAGE_SIZE = 10;

        // GET: Subscriptions
        public ActionResult Index()
        {
            var subscriptions = new List<Subscription>();
            return View("Subscriptions", subscriptions);
        }

        public ActionResult Playlists(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetPlaylistSearchResults(searchString, sortOrder, sortDir);
            return View(result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));

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

        private IQueryable<Playlist> GetPlaylistSearchResults(string searchString, string sortOrder, string sortDir)
        {
            var result = db.Playlists.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                result = result.Where(x => x.Title != null && x.Title.ToLower().Contains(searchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortDir = sortDir;

                if (sortOrder == "title" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Title);
                else if (sortOrder == "title" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Title);
                else if (sortOrder == "publishedAt" && sortDir == "ASC")
                    result = result.OrderBy(x => x.PublishedAt);
                else if (sortOrder == "publishedAt" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.PublishedAt);
                else if (sortOrder == "privacyStatus" && sortDir == "ASC")
                    result = result.OrderBy(x => x.PrivacyStatus);
                else if (sortOrder == "privacyStatus" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.PrivacyStatus);
                else if (sortOrder == "insertedDate" && sortDir == "ASC")
                    result = result.OrderBy(x => x.InsertedDate);
                else if (sortOrder == "insertedDate" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsRemoved);
                else if (sortOrder == "isRemoved" && sortDir == "ASC")
                    result = result.OrderBy(x => x.PrivacyStatus);
                else if (sortOrder == "isRemoved" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsRemoved);
            }
            else
                result = result.OrderBy(x => x.ID);

            return result;
        }

    }
}
