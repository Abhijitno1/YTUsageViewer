using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YTUsageViewer.Models
{
    [Table("CommentsView")]
    public class Comment
    {
        [Key]
        public long Id { get; set; }
        public string CommentId { get; set; }
        public string CommentType { get; set; }
        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public string CommentUrl { get; set; }
        public string CommentText { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm}")]
        public DateTime? CreatedWhen { get; set; }
        public string IsUnavailable { get; set; }
        //public DateTime InsertedDate { get; set; } //ToDo: Currently unavailable in view

    }
}