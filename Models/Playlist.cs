using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YTUsageViewer.Models
{
    public class Playlist
    {
        [Key]
        public int ID { get; set; }
        public string CharId { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm}")]
        public DateTime PublishedAt { get; set; }
        public string PrivacyStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime InsertedDate { get; set; }
        [DisplayFormat(DataFormatString ="{0:dd-MMM-yyyy}")]
        public DateTime? LastUpdatedDate { get; set; }
        public string IsRemoved { get; set; }
    }
}