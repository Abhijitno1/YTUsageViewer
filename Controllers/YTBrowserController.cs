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
        public ActionResult Index(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) pageNumber = 1;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetSubscriptionSearchResults(searchString, sortOrder, sortDir);
            return View("Subscriptions", result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));
        }

        public ActionResult Playlists(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) pageNumber = 1;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetPlaylistSearchResults(searchString, sortOrder, sortDir);
            return View(result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));
        }

        public ActionResult Channels(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) pageNumber = 1;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetChannelSearchResults(searchString, sortOrder, sortDir);
            return View(result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));
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

        private IQueryable<Subscription> GetSubscriptionSearchResults(string searchString, string sortOrder, string sortDir)
        {
            var result = db.Subscriptions.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                result = result.Where(x => x.ChannelTitle != null && x.ChannelTitle.ToLower().Contains(searchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortDir = sortDir;

                if (sortOrder == "channelTitle" && sortDir == "ASC")
                    result = result.OrderBy(x => x.ChannelTitle);
                else if (sortOrder == "channelTitle" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.ChannelTitle);
                else if (sortOrder == "channelId" && sortDir == "ASC")
                    result = result.OrderBy(x => x.ChannelId);
                else if (sortOrder == "channelId" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.ChannelId);
                else if (sortOrder == "insertedDate" && sortDir == "ASC")
                    result = result.OrderBy(x => x.InsertedDate);
                else if (sortOrder == "insertedDate" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.InsertedDate);
                else if (sortOrder == "isRemoved" && sortDir == "ASC")
                    result = result.OrderBy(x => x.IsRemoved);
                else if (sortOrder == "isRemoved" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsRemoved);
            }
            else
                result = result.OrderBy(x => x.ID);

            return result;
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
                    result = result.OrderByDescending(x => x.InsertedDate);
                else if (sortOrder == "isRemoved" && sortDir == "ASC")
                    result = result.OrderBy(x => x.IsRemoved);
                else if (sortOrder == "isRemoved" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsRemoved);
            }
            else
                result = result.OrderBy(x => x.ID);

            return result;
        }

        private IQueryable<Channel> GetChannelSearchResults(string searchString, string sortOrder, string sortDir)
        {
            var result = db.Channels.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                result = result.Where(x => (x.Title != null && x.Title.ToLower().Contains(searchString.ToLower()))
                    || (x.Description != null && x.Description.ToLower().Contains(searchString.ToLower())));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                ViewBag.SortOrder = sortOrder;
                ViewBag.SortDir = sortDir;

                if (sortOrder == "title" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Title);
                else if (sortOrder == "title" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Title);
                else if (sortOrder == "description" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Description);
                else if (sortOrder == "description" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Description);
                else if (sortOrder == "isDeleted" && sortDir == "ASC")
                    result = result.OrderBy(x => x.IsDeleted);
                else if (sortOrder == "isDeleted" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsDeleted);
                else if (sortOrder == "insertedDate" && sortDir == "ASC")
                    result = result.OrderBy(x => x.InsertedDate);
                else if (sortOrder == "insertedDate" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.InsertedDate);
            }
            else
                result = result.OrderBy(x => x.ID);

            return result;
        }
    }
}
