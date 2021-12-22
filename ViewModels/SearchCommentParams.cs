using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTUsageViewer.ViewModels
{
    public class SearchCommentParams
    {
        public string CommentText { get; set; }
        public string CommentType { get; set; }
        public string VideoId { get; set; }
        public string ChannelId { get; set; }
        //public string SortOrder { get; set; }
        //public string SortDirection { get; set; }
    }
}
