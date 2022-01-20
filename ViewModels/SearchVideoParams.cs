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
        public DateTime? PublishedDateFrom { get; set; }
        public DateTime? PublishedDateTo { get; set; }
        public int? DurationGE { get; set; }
        public int? DurationLE { get; set; }
        public DateTime? InsertedDateFrom { get; set; }
        public DateTime? InsertedDateTo { get; set; }
    }
}