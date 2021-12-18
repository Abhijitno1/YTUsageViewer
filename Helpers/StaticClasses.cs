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

        public static IHtmlString GridColumnHeader(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            var genLink = htmlHelper.ActionLink(linkText, actionName, routeValues);
            string newSortOrder = routeValues["sortOrder"] as string;
            //Additional routeValues
            routeValues["searchString"] = htmlHelper.ViewBag.CurrentFilter;
            routeValues["pageNumber"] = htmlHelper.ViewBag.CurrentPage;

            // Build sort image-link
            TagBuilder tb = new TagBuilder("span");
            //tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("class", $"glyphicon { htmlHelper.GetSortDirIcon(newSortOrder).ToHtmlString() } pull-right");
            var linkImg = tb.ToString(TagRenderMode.SelfClosing);

            // return MvcHtmlString. This class implements IHtmlString interface. IHtmlStrings will not be html encoded.
            return new MvcHtmlString(genLink.ToString() + linkImg);
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
    }
}