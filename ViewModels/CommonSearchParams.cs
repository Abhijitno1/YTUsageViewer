using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.ViewModels
{
    public class CommonSearchParams
    {
        public string SortOrder { set; get; }
        public string SortDir { get; set; }
        public int? PageNumber { get; set; }
    }
}