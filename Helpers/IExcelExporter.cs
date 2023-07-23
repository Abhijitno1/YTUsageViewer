using System.Collections.Generic;
using System.IO;

namespace YTUsageViewer.Helpers
{
  public interface IExcelExporter
  {
    byte[] ExportDataAsSpreadsheet<T>(IEnumerable<T> enumerableData);
  }
}