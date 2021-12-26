﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using YTUsageViewer.Helpers;
using YTUsageViewer.Models;
using YTUsageViewer.ViewModels;

namespace YTUsageViewer.Controllers
{
    public class YTBrowserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const int PAGE_SIZE = 10;
        private const string EXPORT_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // GET: Subscriptions
        public ActionResult Index(SearchSubscriptionParams searchParams)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) searchParams.PageNumber = 1;
            ViewBag.CurrentFilter = searchParams;
            searchParams.PageNumber = searchParams.PageNumber ?? 1;

            var result = GetSubscriptionSearchResults(searchParams);
            return View("Subscriptions", result.ToPagedList((int)ViewBag.CurrentFilter.PageNumber, PAGE_SIZE));
        }


        public ActionResult ExportSubscriptions2SpreadsheetML(string searchString, string sortOrder, string sortDir)
        {
            try
            {
                var searchParams = new SearchSubscriptionParams()
                { 
                    ChannelName = searchString,
                    SortOrder = sortOrder,
                    SortDir = sortDir
                };
                var result = GetSubscriptionSearchResults(searchParams);
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


        public ActionResult Playlists(SearchPlaylistParams searchParams)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) searchParams.PageNumber = 1;
            ViewBag.CurrentFilter = searchParams;
            searchParams.PageNumber = searchParams.PageNumber ?? 1;

            var result = GetPlaylistSearchResults(searchParams);
            return View(result.ToPagedList((int)ViewBag.CurrentFilter.PageNumber, PAGE_SIZE));
        }

        public ActionResult PlaylistItems(string playlistId, PlaylistItemSearchCriteria searchCriteria)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) searchCriteria.PageNumber = 1;
            searchCriteria.PageNumber = searchCriteria.PageNumber ?? 1;
            ViewBag.CurrentFilter = searchCriteria;

            var result = GetPlaylistItemsSearchResults(playlistId, searchCriteria);

            var channelsList = result.Select(x => new { CharId = x.VideoOwnerChannelId, Title = x.VideoOwnerChannelName })
                    .Distinct().OrderBy(x => x.Title).ToList();           
            channelsList.Insert(0, new { CharId = (string)null, Title = string.Empty });
            ViewBag.ChannelsList = new SelectList(channelsList, "CharId", "Title", searchCriteria.ChannelId);

            var playListName = db.Playlists.Where(x => x.CharId == playlistId).FirstOrDefault()?.Title;
            ViewBag.PlaylistName = playListName;

            return View(result.ToPagedList(searchCriteria.PageNumber.Value, PAGE_SIZE));
        }

        public ActionResult Channels(SearchSubscriptionParams searchParams)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) searchParams.PageNumber = 1;
            searchParams.PageNumber = searchParams.PageNumber ?? 1;
            ViewBag.CurrentFilter = searchParams;

            var result = GetChannelSearchResults(searchParams);
            return View(result.ToPagedList((int)searchParams.PageNumber, PAGE_SIZE));
        }

        public ActionResult Videos()
        {
            //build empty search parameters list
            ViewBag.CurrentFilter = new SearchVideoParams();

            var channelsList = db.Videos.Select(x => new { x.ChannelId, x.ChannelName })
                .Distinct().OrderBy(x => x.ChannelName).ToList();
            channelsList.Insert(0, new { ChannelId = (string)null, ChannelName = string.Empty });
            ViewBag.ChannelsList = new SelectList(channelsList, "ChannelId", "ChannelName");

            var result = new List<Video>();
            return View(result.ToPagedList(1, 1));
        }

        [HttpPost]
        public ActionResult Videos(SearchVideoParams searchParams)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) searchParams.PageNumber = 1;
            ViewBag.CurrentFilter = searchParams;
            searchParams.PageNumber = searchParams.PageNumber ?? 1;

            var result = GetVideoSearchResults(searchParams);

            var channelsList = result.Select(x => new { x.ChannelId, x.ChannelName })
                .Distinct().OrderBy(x => x.ChannelName).ToList();
            channelsList.Insert(0, new { ChannelId = (string)null, ChannelName = string.Empty });
            ViewBag.ChannelsList = new SelectList(channelsList, "ChannelId", "ChannelName", searchParams.ChannelId);

            return View(result.ToPagedList((int)ViewBag.CurrentFilter.PageNumber, PAGE_SIZE));
        }
        public ActionResult Comments()
        {
            var result= new List<Comment>();
            //build empty search parameters list
            ViewBag.CurrentFilter = new SearchCommentParams();

            var channelsList = db.Comments.Select(x => new { x.ChannelId, x.ChannelTitle }).Distinct().OrderBy(x => x.ChannelTitle).ToList();
            channelsList.Insert(0, new { ChannelId = (string)null, ChannelTitle = string.Empty });
            ViewBag.ChannelsList = new SelectList(channelsList, "ChannelId", "ChannelTitle");

            var videosList = db.Comments.Select(x => new { x.VideoId, x.VideoTitle}).Distinct().OrderBy(x => x.VideoTitle).ToList();
            videosList.Insert(0, new { VideoId = (string)null, VideoTitle = string.Empty });
            ViewBag.VideosList = new SelectList(videosList, "VideoId", "VideoTitle");

            var commentTypesList = db.Comments.Select(x => x.CommentType).Distinct().OrderBy(x => x).ToList();
            commentTypesList.Insert(0, string.Empty);
            ViewBag.CommentTypesList = new SelectList(commentTypesList);

            return View(result.ToPagedList(1, 1));
        }

        [HttpPost]
        public ActionResult Comments(SearchCommentParams searchParams)
        {
            //Reset the page number if new search is initiated by user
            if (!string.IsNullOrEmpty(Request.Params["Search"])) searchParams.PageNumber = 1;
            searchParams.PageNumber = searchParams.PageNumber ?? 1;
            ViewBag.CurrentFilter = searchParams;

            var result = GetCommentsSearchResult(searchParams);

            var channelsList = result.Select(x => new { x.ChannelId, x.ChannelTitle })
                .Distinct().OrderBy(x => x.ChannelTitle).ToList();
            channelsList.Insert(0, new { ChannelId = (string)null, ChannelTitle = string.Empty });
            ViewBag.ChannelsList = new SelectList(channelsList, "ChannelId", "ChannelTitle", searchParams.ChannelId);

            var videosList = result.Select(x => new { x.VideoId, x.VideoTitle })
                .Distinct().OrderBy(x => x.VideoTitle).ToList();
            videosList.Insert(0, new { VideoId = (string)null, VideoTitle = string.Empty });
            ViewBag.VideosList = new SelectList(videosList, "VideoId", "VideoTitle", searchParams.VideoId);

            var commentTypesList = result.Select(x => x.CommentType).Distinct().OrderBy(x => x).ToList();
            commentTypesList.Insert(0, string.Empty);
            ViewBag.CommentTypesList = new SelectList(commentTypesList);

            return View(result.ToPagedList(searchParams.PageNumber.Value, PAGE_SIZE));
        }

        private IQueryable<Subscription> GetSubscriptionSearchResults(SearchSubscriptionParams searchParams)
        {
            var result = db.Subscriptions.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.ChannelName))
            {
                result = result.Where(x => x.ChannelTitle != null && x.ChannelTitle.ToLower().Contains(searchParams.ChannelName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchParams.SortOrder))
            {
                var sortOrder = searchParams.SortOrder;
                var sortDir = searchParams.SortDir;

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

        private IQueryable<Playlist> GetPlaylistSearchResults(SearchPlaylistParams searchParams)
        {
            var result = db.Playlists.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.PlaylistName))
            {

                result = result.Where(x => x.Title != null && x.Title.ToLower().Contains(searchParams.PlaylistName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchParams.SortOrder))
            {
                var sortOrder = searchParams.SortOrder;
                var sortDir = searchParams.SortDir;

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

        private IQueryable<Channel> GetChannelSearchResults(SearchSubscriptionParams searchParams)
        {
            var result = db.Channels.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.ChannelName))
            {
                result = result.Where(x => (x.Title != null && x.Title.ToLower().Contains(searchParams.ChannelName.ToLower()))
                    || (x.Description != null && x.Description.ToLower().Contains(searchParams.ChannelName.ToLower())));
            }

            if (!string.IsNullOrEmpty(searchParams.SortOrder))
            {
                var sortOrder = searchParams.SortOrder;
                var sortDir = searchParams.SortDir;

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

        private IEnumerable<Video> GetVideoSearchResults(SearchVideoParams searchParams)
        {
            IEnumerable<Video> result = db.Videos.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.VideoName))
            {
                result = result.Where(x => (x.Title != null && x.Title.ToLower().Contains(searchParams.VideoName.ToLower()))
                    || (x.Description != null && x.Description.ToLower().Contains(searchParams.VideoName.ToLower())));
            }

            if (!string.IsNullOrEmpty(searchParams.ChannelId))
            {
                result = result.Where(x => x.ChannelId != null && x.ChannelId.Equals(searchParams.ChannelId));
            }

            if (searchParams.IsDeleted)
            {
                result = result.Where(x => x.IsDeleted != null && x.IsDeleted.Equals("Y"));
            }

            foreach (var convertItm in result)
            {
                convertItm.DurationSpan = HelperMethods.ConvertDuration2TimeSpan(convertItm.Duration);
            }

            if (!string.IsNullOrEmpty(searchParams.SortOrder))
            {
                string sortOrder = searchParams.SortOrder, sortDir = searchParams.SortDir;
                if (sortOrder == "title" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Title);
                else if (sortOrder == "title" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Title);
                else if (sortOrder == "description" && sortDir == "ASC")
                    result = result.OrderBy(x => x.Description);
                else if (sortOrder == "description" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.Description);
                else if (sortOrder == "channelName" && sortDir == "ASC")
                    result = result.OrderBy(x => x.ChannelName);
                else if (sortOrder == "channelName" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.ChannelName);
                else if (sortOrder == "duration" && sortDir == "ASC")
                    result = result.OrderBy(x => x.DurationSpan);
                else if (sortOrder == "duration" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.DurationSpan);
                else if (sortOrder == "publishedAt" && sortDir == "ASC")
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

        private IEnumerable<PlaylistItem> GetPlaylistItemsSearchResults(string playlistId, PlaylistItemSearchCriteria searchCriteria)
        {
            var joinResult = from pl in db.PlaylistItems
                      join ch in db.Channels on pl.VideoOwnerChannelId equals ch.CharId
                      where pl.PlaylistId == playlistId
                      select new { pl, ch };
            joinResult.ToList().ForEach(x => x.pl.VideoOwnerChannelName = x.ch.Title);

            //var result = db.PlaylistItems.AsQueryable().Where(x => x.PlaylistId == playlistId);
            var result = joinResult.Select(x => x.pl);

            if (!string.IsNullOrEmpty(searchCriteria.ItemName))
            {
                result = result.Where(x => x.Title != null && x.Title.ToLower().Contains(searchCriteria.ItemName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchCriteria.ChannelId))
            {
                result = result.Where(x => x.VideoOwnerChannelId != null && x.VideoOwnerChannelId.Equals(searchCriteria.ChannelId));
            }

            if (!string.IsNullOrEmpty(searchCriteria.SortOrder))
            {
                var sortOrder = searchCriteria.SortOrder;
                var sortDir = searchCriteria.SortDir;

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

        private IEnumerable<Comment> GetCommentsSearchResult(SearchCommentParams searchParams)
        {
            IEnumerable<Comment> result = db.Comments.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.CommentText))
            {
                result = result.Where(x => x.CommentText != null && x.CommentText.ToLower().Contains(searchParams.CommentText.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchParams.CommentType))
            {
                result = result.Where(x => x.CommentType != null && x.CommentType.ToLower().Contains(searchParams.CommentType.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchParams.ChannelId))
            {
                result = result.Where(x => x.ChannelId != null && x.ChannelId.Equals(searchParams.ChannelId));
            }
            if (!string.IsNullOrEmpty(searchParams.VideoId))
            {
                result = result.Where(x => x.VideoId != null && x.VideoId.Equals(searchParams.VideoId));
            }

            if (!string.IsNullOrEmpty(searchParams.SortOrder))
            {
                string sortOrder = searchParams.SortOrder, sortDir = searchParams.SortDir;

                if (sortOrder == "commentType" && sortDir == "ASC")
                    result = result.OrderBy(x => x.CommentType);
                else if (sortOrder == "commentType" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.CommentType);
                else if (sortOrder == "videoTitle" && sortDir == "ASC")
                    result = result.OrderBy(x => x.VideoTitle);
                else if (sortOrder == "videoTitle" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.VideoTitle);
                else if (sortOrder == "channelTitle" && sortDir == "ASC")
                    result = result.OrderBy(x => x.ChannelTitle);
                else if (sortOrder == "channelTitle" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.ChannelTitle);
                else if (sortOrder == "commentText" && sortDir == "ASC")
                    result = result.OrderBy(x => x.CommentText);
                else if (sortOrder == "commentText" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.CommentText);
                else if (sortOrder == "createdWhen" && sortDir == "ASC")
                    result = result.OrderBy(x => x.CreatedWhen);
                else if (sortOrder == "createdWhen" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.CreatedWhen);
                else if (sortOrder == "isUnavailable" && sortDir == "ASC")
                    result = result.OrderBy(x => x.IsUnavailable);
                else if (sortOrder == "isUnavailable" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsUnavailable);
                /* placeholder for sort on InstertedDate
                else if (sortOrder == "isDeleted" && sortDir == "ASC")
                    result = result.OrderBy(x => x.IsDeleted);
                else if (sortOrder == "isDeleted" && sortDir == "DESC")
                    result = result.OrderByDescending(x => x.IsDeleted);
                */
            }
            else
                result = result.OrderBy(x => x.Id);

            return result.ToList();
        }
    }
}
