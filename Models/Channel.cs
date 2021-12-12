using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YTUsageViewer.Models
{
    public class Channel
    {
        [Key]
        public int ID { get; set; }
        public string CharId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IsDeleted { get; set; }
        public string InsertedDate { get; set; }

    }
}