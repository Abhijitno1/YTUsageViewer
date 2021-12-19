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

            var result = GetPlaylistItemsSearchResults(playlistId, searchString, sortOrder, sortDir);

            var channelsList = result.Select(x => new { CharId = x.VideoOwnerChannelId, Title = x.VideoOwnerChannelName })
                    .Distinct().OrderBy(x => x.Title).ToList();           
            channelsList.Insert(0, new { CharId = "-1", Title = "" });
            ViewBag.ChannelsList = new SelectList(channelsList, "CharId", "Title");

            var playListName = db.Playlists.Where(x => x.CharId == playlistId).FirstOrDefault()?.Title;
            ViewBag.PlaylistName = playListName;

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

        private IEnumerable<Video> GetVideoSearchResults(string searchString, string sortOrder, string sortDir)
        {
            var result = db.Videos.AsQueryable();

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

        private IEnumerable<PlaylistItem> GetPlaylistItemsSearchResults(string playlistId, string searchString, string sortOrder, string sortDir)
        {
            var joinResult = from pl in db.PlaylistItems
                      join ch in db.Channels on pl.VideoOwnerChannelId equals ch.CharId
                      where pl.PlaylistId == playlistId
                      select new { pl, ch };
            joinResult.ToList().ForEach(x => x.pl.VideoOwnerChannelName = x.ch.Title);

            //var result = db.PlaylistItems.AsQueryable().Where(x => x.PlaylistId == playlistId);
            var result = joinResult.Select(x => x.pl);

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
                if (sortOrder == "publishedAt" && sortDir == "ASC")
                    result = result.OrderBy(x => x.PublishedAt);
                else if (sortOrder == "publishedAt" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.PublishedAt);
                if (sortOrder == "videoOwnerChannel" && sortDir == "ASC")
                    result = result.OrderBy(x => x.VideoOwnerChannelName);
                else if (sortOrder == "videoOwnerChannel" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.VideoOwnerChannelName);
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
                result = result.OrderBy(x => x.Id);

            return result.ToList();
        }

    }
}
