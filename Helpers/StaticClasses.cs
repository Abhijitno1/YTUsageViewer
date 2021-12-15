using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YTUsageViewer.Helpers
{
    public class YTBrowserMenu
    {
        public const string Subscriptions = "Subscriptions";
        public const string Playlists = "Playlists";
        public const string Channels = "Channels";
        public const string Videos = "Videos";
        public const string Comments = "Comments";
    }

    public static class CustomHelpers
    {
        public static IHtmlString GetSortDirIcon(this HtmlHelper helper, string sortOrderCheck)
        {
            //RefEx: https://dotnettutorials.net/lesson/custom-html-helpers-mvc/
            var retVal = String.Empty;
            switch (helper.ViewBag.SortDir)
            {
                case "ASC":
                    retVal = helper.ViewBag.SortOrder == sortOrderCheck ? "glyphicon-sort-by-attributes" : string.Empty;
                    break;
                case "DESC":
                    retVal = helper.ViewBag.SortOrder == sortOrderCheck ? "glyphicon-sort-by-attributes-alt" : string.Empty;
                    break;
            }
            return new MvcHtmlString(retVal);
        }
    }
}