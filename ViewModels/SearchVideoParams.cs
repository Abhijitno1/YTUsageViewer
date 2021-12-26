using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.ViewModels
{
    public class SearchVideoParams : CommonSearchParams
    {
        public string VideoName { get; set; }
        public string ChannelId { get; set; }
        public bool IsDeleted { get; set; }
    }
}