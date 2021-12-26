using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTUsageViewer.Models;

namespace YTUsageViewer.ViewModels
{
    public class PlaylistItemSearchCriteria : CommonSearchParams
    {
        public string ItemName { get; set; }
        public string ChannelId { get; set; }
    }
}
