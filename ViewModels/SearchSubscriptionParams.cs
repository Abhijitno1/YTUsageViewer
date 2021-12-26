using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.ViewModels
{
    public class SearchSubscriptionParams : CommonSearchParams
    {
        public string ChannelName { get; set; }
    }
}