using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.Models
{
    public class CommentsSearch
    {
        public string Subscription { get; set; }
        public string Playlist { get; set; }
        public string Channel { get; set; }
        public string VideoName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CommentText { get; set; }
    }
}