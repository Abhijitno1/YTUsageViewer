using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.Models
{
    public class Playlist
    {
        public int ID { get; set; }
        public string CharId { get; set; }
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; }
        public string PrivacyStatus { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsRemoved { get; set; }
    }
}