using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YTUsageViewer.Models
{
    [Table("VideosUIView")]
    public class Video
    {
        [Key]
        public long ID { get; set; }
        public string CharId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Duration { get; set; }
        [NotMapped]
        public TimeSpan? DurationSpan { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? PublishedAt { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime InsertedDate { get; set; }
        public string IsDeleted { get; set; }
    }
}