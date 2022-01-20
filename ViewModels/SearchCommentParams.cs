using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTUsageViewer.ViewModels
{
    public class SearchCommentParams: CommonSearchParams
    {
        public string CommentText { get; set; }
        public string CommentType { get; set; }
        public string VideoId { get; set; }
        public string ChannelId { get; set; }
        public DateTime? CreatedWhenFrom { get; set; }
        public DateTime? CreatedWhenTo { get; set; }
        public bool IsUnavailable { get; set; }
    }
}
