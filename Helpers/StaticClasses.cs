using PagedList;
using PMVC = PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

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

        public static IHtmlString GetSortDirIconNew(this HtmlHelper helper, string sortOrderCheck)
        {
            //RefEx: https://dotnettutorials.net/lesson/custom-html-helpers-mvc/
            var retVal = String.Empty;
            switch (helper.ViewBag.CurrentFilter.SortDir)
            {
                case "ASC":
                    retVal = helper.ViewBag.CurrentFilter.SortOrder == sortOrderCheck ? "glyphicon-sort-by-attributes" : string.Empty;
                    break;
                case "DESC":
                    retVal = helper.ViewBag.CurrentFilter.SortOrder == sortOrderCheck ? "glyphicon-sort-by-attributes-alt" : string.Empty;
                    break;
            }
            return new MvcHtmlString(retVal);
        }

        public static IHtmlString GridColumnHeader(this HtmlHelper htmlHelper, string linkText, string newSortOrder)
        {
            var genLink = @"<a href='Javascript:sortGrid(""" + newSortOrder + @""")'>" + linkText + "</a>";

            // Build sort image-link
            TagBuilder tb = new TagBuilder("span");
            //tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("class", $"glyphicon { htmlHelper.GetSortDirIconNew(newSortOrder).ToHtmlString() } pull-right");
            var linkImg = tb.ToString(TagRenderMode.SelfClosing);

            // return MvcHtmlString. This class implements IHtmlString interface. IHtmlStrings will not be html encoded.
            return new MvcHtmlString(genLink + linkImg);
        }


        public static IHtmlString GridColumnHeader(this HtmlHelper htmlHelper, string linkText, string actionName, string newSortOrder, string newSortDir)
        {
            //RefEx: https://stackoverflow.com/questions/25763232/create-a-custom-actionlink
            var routeValues = new {
                sortOrder = newSortOrder,
                sortDir = newSortDir,
                searchString = htmlHelper.ViewBag.CurrentFilter,
                pageNumber = htmlHelper.ViewBag.CurrentPage
            };
            //routeValues.Add("tabMenu", tabMenu);
            var genLink = htmlHelper.ActionLink(linkText, actionName, routeValues);
            // Build sort image-link
            TagBuilder tb = new TagBuilder("span");
            //tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("class", $"glyphicon { htmlHelper.GetSortDirIcon(newSortOrder).ToHtmlString() } pull-right");
            var linkImg = tb.ToString(TagRenderMode.SelfClosing);

            // return MvcHtmlString. This class implements IHtmlString interface. IHtmlStrings will not be html encoded.
            return new MvcHtmlString(genLink.ToString() + linkImg);
        }

        public static IHtmlString TruncatedGridColumn(this HtmlHelper htmlHelper, string fullText, int maxColCharLength = 100)
        {
            fullText = fullText ?? string.Empty;
            fullText = fullText.Trim();

            var displayText = fullText;
            var magnifier = string.Empty;
            if (fullText.Length > maxColCharLength)
            {
                displayText = fullText.Substring(0, maxColCharLength) + "...";

                // Build magnifier image-link
                TagBuilder tb = new TagBuilder("a");
                tb.AddCssClass("pull-right");
                tb.Attributes.Add("href", "#");
                tb.Attributes.Add("data-toggle", "popover");
                //tb.Attributes.Add("data-trigger", "focus");
                tb.Attributes.Add("data-content", fullText);
                TagBuilder tbSpan = new TagBuilder("span");
                tbSpan.AddCssClass("glyphicon");
                tbSpan.AddCssClass("glyphicon-zoom-in");
                tb.InnerHtml = tbSpan.ToString();
                magnifier = tb.ToString();
            }

            return new MvcHtmlString(displayText + magnifier);
        }

        public static IHtmlString JSPagedListPager(this HtmlHelper helper, IPagedList list)
        {
            return PMVC.HtmlHelper.PagedListPager(helper, list, pageNumber => "Javascript: pageGrid(" + pageNumber + ")");
        }
    }

    public static class HelperMethods
    {
        public static TimeSpan? ConvertDuration2TimeSpan(string duration)
        {
            TimeSpan? result = null;
            //var match = Regex.Match(duration, @"PT(\d*?H*)(\d+M)(\d*S*)");
            int hours = 0, minutes = 0, seconds = 0;
            var ptPos = duration.IndexOf("PT") + 2;
            var hPos = duration.IndexOf("H");
            var mPos = duration.IndexOf("M");
            var sPos = duration.IndexOf("S");

            if (ptPos == 1) return result;  //Unsupported format

            if (hPos > -1)
            {
                hours = int.Parse(duration.Substring(ptPos, hPos - ptPos));
                if (mPos > -1)
                {
                    minutes = int.Parse(duration.Substring(hPos + 1, mPos - hPos - 1));
                    if (sPos > -1) seconds = int.Parse(duration.Substring(mPos + 1, sPos - mPos - 1));
                }
            }
            else
            {
                if (mPos > -1)
                {
                    minutes = int.Parse(duration.Substring(ptPos, mPos - ptPos));
                    if (sPos > -1) seconds = int.Parse(duration.Substring(mPos + 1, sPos - mPos - 1));
                }
                else if (sPos > -1)
                {
                    seconds = int.Parse(duration.Substring(ptPos, sPos - ptPos));
                }
            }
            if (hours > 0 || minutes > 0 || seconds > 0)
                result = new TimeSpan(hours, minutes, seconds);

            return result;
        }


    }
}