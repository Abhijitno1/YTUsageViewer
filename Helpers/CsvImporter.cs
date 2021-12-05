using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using YTUsageViewer.Models;

namespace YTUsageViewer.Helpers
{
    public class CsvImporter
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        public DataTable GetDataTableFromCSVFile(Stream inputStream, bool hasFieldsEnclosedInQuotes = false)
        {
            var csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(inputStream))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = hasFieldsEnclosedInQuotes;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:{0}", ex.Message);
            }
            return csvData;
        }

        public void SaveContacts2DB(DataTable contacts)
        {
            var columnExists = new Dictionary<string, bool>() 
            {
                {"FirstName", contacts.Columns.Contains("FirstName") },
                {"LastName", contacts.Columns.Contains("LastName") },
                {"PhoneHome", contacts.Columns.Contains("PhoneHome") },
                {"PhoneMobile", contacts.Columns.Contains("PhoneMobile") },
                {"PhoneWork", contacts.Columns.Contains("PhoneWork") },
                {"PreferredPhone", contacts.Columns.Contains("PreferredPhone") },
                {"Email", contacts.Columns.Contains("Email") }
            };

            foreach (DataRow dataRow in contacts.Rows)
            {
                var newContact = new Contact();
                if (columnExists["FirstName"])
                    newContact.FirstName = Convert.ToString(dataRow["FirstName"]);
                if (columnExists["LastName"])
                    newContact.LastName = Convert.ToString(dataRow["LastName"]);
                if (columnExists["PhoneHome"])
                    newContact.PhoneHome = Convert.ToString(dataRow["PhoneHome"]);
                if (columnExists["PhoneMobile"])
                    newContact.PhoneMobile = Convert.ToString(dataRow["PhoneMobile"]);
                if (columnExists["PhoneWork"])
                    newContact.PhoneWork = Convert.ToString(dataRow["PhoneWork"]);
                if (columnExists["PreferredPhone"])
                    newContact.PreferredPhone = Convert.ToString(dataRow["PreferredPhone"]);
                if (columnExists["Email"])
                    newContact.Email = Convert.ToString(dataRow["Email"]);

                dbContext.Contacts.Add(newContact);
            }
            dbContext.SaveChanges();
        }
    }
}