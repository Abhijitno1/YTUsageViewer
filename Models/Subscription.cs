﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace YTUsageViewer.Models
{
    public class Subscription
    {
        [Key]
        public int ID { get; set; }
        public string CharId { get; set; }
        public string ChannelTitle { get; set; }
        public string ChannelId { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsRemoved { get; set; }

    }
}