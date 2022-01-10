//using Microsoft.Analytics.Interfaces;
//using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YTUsageViewer.ViewModels
{
    public class DatePickerModel
    {
        public string InputName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? SelectedDate { get; set; }
    }
}