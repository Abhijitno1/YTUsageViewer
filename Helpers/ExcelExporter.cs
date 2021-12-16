using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;
using YTUsageViewer.Models;
using System.Xml;
using System.Text;
using System.Data;

namespace YTUsageViewer.Helpers
{
    public class ExcelExporter
    {
        public Stream ExportDataAsSpreadsheet<T>(IEnumerable<T> enumerableData)
        {
            var dataSet = GetDataSet(enumerableData);
            var dataXml = "<?xml version=\"1.0\"?>";
            dataXml += dataSet.GetXml();

            //System.Diagnostics.Debug.Write("Input\n" + dataXml);
            //var dataFilePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "ContactsData.xml");
            //var xmlReader = XmlReader.Create(dataFilePath);
            var reader = new StringReader(dataXml);
            var xmlReader = XmlReader.Create(reader);

            //return reader.BaseStream;
            return Transform2SpreadsheetMLUsingXSL(xmlReader);
        }

        private DataSet GetDataSet<T>(IEnumerable<T> myEnumerable)
        {
            DataSet ds = new DataSet();
            Type type = myEnumerable.GetType().GetGenericArguments()[0];

            if (type == typeof(Contact))
                ds = GetDataSet((IEnumerable<Contact>)myEnumerable);

            return ds;
        }

        private Stream Transform2SpreadsheetMLUsingXSL(XmlReader reader)
        {
            //Assumption: input stream is an XML Document
            var argsList = new XsltArgumentList();
            var document = new XPathDocument(reader);
            StringWriter writer = new StringWriter();
            XslCompiledTransform transform = new XslCompiledTransform();

            var xslFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Templates"), "ContactsDataConverter.xsl");
            transform.Load(xslFilePath);
            transform.Transform(document, argsList, writer);

            var transformed = writer.ToString();
            //System.Diagnostics.Debug.Write("Output\n" + transformed.ToString());
            return GetStreamFromString(transformed);
        }

        private Stream GetStreamFromString(string input)
        {
            return new MemoryStream(Encoding.Default.GetBytes(input));
        }

        #region Entity Collection specific code
        public Stream ExportContactsTabDelimited(IEnumerable<Contact> data)
        {
            //Ref: https://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
            //Ref2: https://www.aspsnippets.com/Articles/Export-to-CSV-in-ASPNet-MVC.aspx

            StringBuilder content = new StringBuilder();
            string tab = "";
            var columnNames = new[] { "First Name", "Last Name", "Mobile Phone", "Work Phone", "Home Phone", "Preferred Phone", "Email" };
            foreach (var dc in columnNames)
            {
                content.Append(tab + dc);
                tab = "\t";
            }
            content.Append("\n");
            foreach (var dr in data)
            {
                content.Append(dr.FirstName);
                content.Append(tab + dr.LastName);
                content.Append(tab + dr.PhoneMobile);
                content.Append(tab + dr.PhoneWork);
                content.Append(tab + dr.PhoneHome);
                content.Append(tab + dr.PreferredPhone);
                content.Append(tab + dr.Email);
                content.Append("\n");
            }
            return GetStreamFromString(content.ToString());
        }

        private DataSet GetDataSet(IEnumerable<Contact> contacts)
        {
            var output = new DataSet();
            var dtTable = new DataTable("Contacts");
            dtTable.Columns.Add("FirstName");
            dtTable.Columns.Add("LastName");
            dtTable.Columns.Add("PhoneMobile");
            dtTable.Columns.Add("PhoneWork");
            dtTable.Columns.Add("PhoneHome");
            dtTable.Columns.Add("PreferredPhone");
            dtTable.Columns.Add("Email");
            output.Tables.Add(dtTable);
            foreach (var dr in contacts)
            {
                var newRow = dtTable.NewRow();
                newRow["FirstName"] = dr.FirstName;
                newRow["LastName"] = dr.LastName;
                newRow["PhoneMobile"] = dr.PhoneMobile;
                newRow["PhoneWork"] = dr.PhoneWork;
                newRow["PhoneHome"] = dr.PhoneHome;
                newRow["PreferredPhone"] = dr.PreferredPhone;
                newRow["Email"] = dr.Email;
                dtTable.Rows.Add(newRow);
            }
            return output;
        }
        #endregion Entity Collection specific code
    }

}