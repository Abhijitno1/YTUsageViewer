using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTUsageViewer.Models;

namespace YTUsageViewer.ViewModels
{
    public class PlaylistItemViewModel
    {
        public PagedList.IPagedList<PlaylistItem> PlaylistItems { get; set; }
        //ToDo: Implemention in future

    }

    public class PlaylistItemSearchCriteria
    {
        public string Title { get; set; }
        public string ChannelId { get; set; }
    }
}
