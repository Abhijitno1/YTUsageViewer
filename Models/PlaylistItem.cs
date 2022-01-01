using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTUsageViewer.Models
{
    public class PlaylistItem
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm}")]
        public DateTime? PublishedAt { get; set; }
        public string PlaylistId { get; set; }
        [NotMapped]
        public string PlaylistName { get; set; }
        public string VideoId { get; set; }
        public string VideoOwnerChannelId { get; set; }
        [NotMapped]
        public string VideoOwnerChannelName { get; set; }
        public string CharId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime InsertedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? LastUpdatedDate { get; set; }
        public string IsRemoved { get; set; }
    }
}
