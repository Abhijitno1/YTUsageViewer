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
    public AddParametersDelegate AddParameters { get; set; }

    public byte[] ExportDataAsSpreadsheet<T>(IEnumerable<T> enumerableData)
    {
      var dataSet = GetDataSet(enumerableData);
      var dataTable = dataSet.Tables[0];
      using (var wb = new XLWorkbook())
      {
        var sheet = wb.AddWorksheet(dataTable, dataTable.TableName);
        sheet.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;
        sheet.Row(1).CellsUsed().Style.Font.FontColor = XLColor.White;
        sheet.Row(1).Style.Font.Bold = true;
        sheet.Columns().AdjustToContents();
        foreach (var column in sheet.Columns())
        {
          //Ref: https://stackoverflow.com/questions/15198445/how-to-make-excel-wrap-text-in-formula-cell-with-closedxml
          if (column.Width > 120)
          {
            column.Width = 120;
            column.CellsUsed().Style.Alignment.WrapText = true;
          }
        }
        if (AddParameters != null)
          AddParameters.Invoke(sheet);

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
      else if (type == typeof(Channel))
        ds = GetDataSet((IEnumerable<Channel>)myEnumerable);

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

    private DataSet GetDataSet(IEnumerable<Channel> channels)
    {
      var output = new DataSet();
      var dtTable = new DataTable("Channels");
      dtTable.Columns.Add("Channel Id");
      dtTable.Columns.Add("Title");
      dtTable.Columns.Add("Description");
      dtTable.Columns.Add("Inserted Date");
      dtTable.Columns.Add("Is Deleted");
      output.Tables.Add(dtTable);
      foreach (var dr in channels)
      {
        var newRow = dtTable.NewRow();
        newRow["Channel Id"] = dr.CharId;
        newRow["Title"] = dr.Title;
        newRow["Description"] = dr.Description;
        newRow["Inserted Date"] = dr.InsertedDate;
        newRow["Is Deleted"] = dr.IsDeleted;
        dtTable.Rows.Add(newRow);
      }
      return output;
    }

  }
}