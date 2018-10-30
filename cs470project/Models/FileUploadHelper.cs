using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace cs470project.Models
{
    public class FileUploadHelper
    {
        /*
         * Author: Zak Zinda
         * Date Updated: 10.30.18
         * Description: Uses Microsoft's built-in Excel reference to read a .csv, .xls, or .xlsx file
         *              and parse each row value into a list. For now, we assume the cells to be parsed are
         *              located in the first sheet and the first column, and that the file extension
         *              has already been validated.
         *              Note: Excel is 1-indexed not 0-indexed.
         * Parameters: string fileName - Local file name for Excel's file access.
         *             bool hasHeader - Determines the starting row for the parser. If true, start
         *                              at row 2, if false, start at row 1.
         */
        public List<string> ParseFileToList(string fileName, bool hasHeader)
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileName);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                List<string> list = new List<string>();
                // If hasHeader is true, start reading cells at row 2. If false, start at row 1.
                int startingIndex = hasHeader ? 2 : 1;

                for (int i = startingIndex; i <= rowCount; i++)
                {
                    if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
                    {
                        list.Add(xlRange.Cells[i, 1].Value2.ToString());
                    }
                }

                // Release, close, and shut down Excel processes.
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);

                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}