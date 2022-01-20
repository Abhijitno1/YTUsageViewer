using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.ViewModels
{
    public class SearchSubscriptionParams : CommonSearchParams
    {
        public string ChannelName { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime? InsertedDateFrom { get; set; }
        public DateTime? InsertedDateTo { get; set; }
    }
}