using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.ViewModels
{
    public class PlayList4VideoViewModel
    {
        public string VideoId { get; set; }
        public string VideoName { get; set; }
        public IEnumerable<string> PlaylistNames { get; set;}
    }
}