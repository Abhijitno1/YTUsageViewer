using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace YTUsageViewer.Helpers
{
  public delegate void AddParametersDelegate(IXLWorksheet worksheet);
  public interface IExcelExporter
  {
    AddParametersDelegate AddParameters { get; set; }
    byte[] ExportDataAsSpreadsheet<T>(IEnumerable<T> enumerableData);
  }
}