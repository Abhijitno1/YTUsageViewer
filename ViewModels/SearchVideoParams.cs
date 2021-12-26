using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.ViewModels
{
    public class SearchVideoParams
    {
        public string VideoName { get; set; }
        public string ChannelId { get; set; }
        public bool IsDeleted { get; set; }

        public string SortOrder { set; get; }
        public string SortDir { get; set; }
        public int? PageNumber { get; set; }
    }
}