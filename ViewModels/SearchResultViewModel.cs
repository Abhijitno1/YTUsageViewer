using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.ViewModels
{
  public class SearchResultViewModel<T>
  {
    public int total { get; set; }
    public List<T> data { get; set; }
  }
}