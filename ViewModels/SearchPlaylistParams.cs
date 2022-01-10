using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTUsageViewer.ViewModels
{
    public class SearchPlaylistParams: CommonSearchParams
    {
        public string PlaylistName { get; set; }
        public string PrivacyStatus { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime? PublishedDateFrom { get; set; }
        public DateTime? PublishedDateTo { get; set; }
        public DateTime? InsertedDateFrom { get; set; }
        public DateTime? InsertedDateTo { get; set; }
    }
}
