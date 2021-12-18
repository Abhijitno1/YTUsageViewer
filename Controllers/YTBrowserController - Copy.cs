using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Helpers;
using YTUsageViewer.Models;

namespace YTUsageViewer.Controllers
{
    public class YTBrowserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const int PAGE_SIZE = 10;
        private const string EXPORT_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // GET: Subscriptions
        public ActionResult Index(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) pageNumber = 1;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetSubscriptionSearchResults(searchString, sortOrder, sortDir);
            return View("Subscriptions", result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));
        }

        public ActionResult ExportSubscriptions2SpreadsheetML(string searchString, string sortOrder, string sortDir)
        {
            try
            {
                var result = GetSubscriptionSearchResults(searchString, sortOrder, sortDir);
                ExcelExporter xlXporter = new ExcelExporter();
                var outputStream = xlXporter.ExportDataAsSpreadsheet(result);
                string attachmentName = "Subscriptions-xls.xml";
                return File(outputStream, EXPORT_CONTENT_TYPE, attachmentName);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Playlists(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) pageNumber = 1;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetPlaylistSearchResults(searchString, sortOrder, sortDir);
            return View(result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));
        }

        public ActionResult PlaylistItems(string playlistId, string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) pageNumber = 1;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetPlaylistItemsSearchResults(searchString, sortOrder, sortDir);
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

        public ActionResult Videos(string searchString, string sortOrder, string sortDir, int? pageNumber)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) pageNumber = 1;
            ViewBag.CurrentPage = pageNumber ?? 1;

            var result = GetVideoSearchResults(searchString, sortOrder, sortDir);
            return View(result.ToPagedList((int)ViewBag.CurrentPage, PAGE_SIZE));
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

        private IEnumerable<PlaylistItem> GetVideoSearchResults(string searchString, string sortOrder, string sortDir)
        {
            var result = db.PlaylistItems.AsQueryable();

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
                else if (sortOrder == "videoId" && sortDir == "ASC")
                    result = result.OrderBy(x => x.VideoId);
                else if (sortOrder == "videoId" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.VideoId);
                else if (sortOrder == "videoOwnerChannelId" && sortDir == "ASC")
                    result = result.OrderBy(x => x.VideoOwnerChannelId);
                else if (sortOrder == "videoOwnerChannelId" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.VideoOwnerChannelId);
                if (sortOrder == "insertedDate" && sortDir == "ASC")
                    result = result.OrderBy(x => x.InsertedDate);
                else if (sortOrder == "insertedDate" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.InsertedDate);
                else if (sortOrder == "lastUpdatedDate" && sortDir == "ASC")
                    result = result.OrderBy(x => x.LastUpdatedDate);
                else if (sortOrder == "lastUpdatedDate" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.LastUpdatedDate);
                else if (sortOrder == "isRemoved" && sortDir == "ASC")
                    result = result.OrderBy(x => x.IsRemoved);
                else if (sortOrder == "isRemoved" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsRemoved);
            }
            else
                result = result.OrderBy(x => x.Id);

            return result.ToList();
        }

        private IEnumerable<PlaylistItem> GetPlaylistItemsSearchResults(string searchString, string sortOrder, string sortDir)
        {
            var result = db.PlaylistItems.AsQueryable();

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
                else if (sortOrder == "description" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Description);
                else if (sortOrder == "description" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Description);

                if (sortOrder == "channelId" && sortDir == "ASC")
                    result = result.OrderBy(x => x.ChannelId);
                else if (sortOrder == "channelId" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.ChannelId);
                if (sortOrder == "duration" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Duration);
                else if (sortOrder == "duration" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Duration);
                if (sortOrder == "publishedAt" && sortDir == "ASC")
                    result = result.OrderBy(x => x.PublishedAt);
                else if (sortOrder == "publishedAt" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.PublishedAt);
                else if (sortOrder == "insertedDate" && sortDir == "ASC")
                    result = result.OrderBy(x => x.InsertedDate);
                else if (sortOrder == "insertedDate" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.InsertedDate);
                else if (sortOrder == "isDeleted" && sortDir == "ASC")
                    result = result.OrderBy(x => x.IsDeleted);
                else if (sortOrder == "isDeleted" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsDeleted);
            }
            else
                result = result.OrderBy(x => x.ID);

            return result.ToList();
        }
    }
}
