using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using YTUsageViewer.Models;

namespace YTUsageViewer.Helpers
{
  public class ExcelExporterWithCXml : IExcelExporter
  {
    public byte[] ExportDataAsSpreadsheet<T>(IEnumerable<T> enumerableData)
    {
      var dataSet = GetDataSet(enumerableData);
      var dataTable = dataSet.Tables[0];
      using (var wb = new XLWorkbook())
      {
        var sheet = wb.AddWorksheet(dataTable, dataTable.TableName);
        sheet.Column(1).Style.Font.FontColor = XLColor.DarkBlue;
        sheet.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;
        sheet.Row(1).CellsUsed().Style.Font.FontColor = XLColor.White;
        sheet.Row(1).Style.Font.Bold = true;
        sheet.Columns().AdjustToContents();

        using (MemoryStream ms = new MemoryStream())
        {
          wb.SaveAs(ms);
          return ms.ToArray();
        }
      }
    }

    private DataSet GetDataSet<T>(IEnumerable<T> myEnumerable)
    {
      DataSet ds = new DataSet();
      Type type = myEnumerable.GetType().GetGenericArguments()[0];

      if (type == typeof(Subscription))
        ds = GetDataSet((IEnumerable<Subscription>)myEnumerable);

      return ds;
    }

    private DataSet GetDataSet(IEnumerable<Subscription> subscriptions)
    {
      var output = new DataSet();
      var dtTable = new DataTable("Subscriptions");
      dtTable.Columns.Add("Sub Id");
      dtTable.Columns.Add("Channel Id");
      dtTable.Columns.Add("Channel Title");
      dtTable.Columns.Add("Inserted Date");
      dtTable.Columns.Add("Is Removed");
      output.Tables.Add(dtTable);
      foreach (var dr in subscriptions)
      {
        var newRow = dtTable.NewRow();
        newRow["Sub Id"] = dr.CharId;
        newRow["Channel Id"] = dr.ChannelId;
        newRow["Channel Title"] = dr.ChannelTitle;
        newRow["Inserted Date"] = dr.InsertedDate;
        newRow["Is Removed"] = dr.IsRemoved;
        dtTable.Rows.Add(newRow);
      }
      return output;
    }

  }
}