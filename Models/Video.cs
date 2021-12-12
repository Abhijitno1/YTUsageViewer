using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YTUsageViewer.Models
{
    public class Video
    {
        [Key]
        public int ID { get; set; }
        public string CharId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelId { get; set; }
        public string Duration { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime InsertedDate { get; set; }
        public string IsDeleted { get; set; }
    }
}