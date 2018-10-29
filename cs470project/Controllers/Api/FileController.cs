using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace cs470project.Controllers.Api
{
    public class FileController : ApiController
    {

        /**
         *  Author:       Zak Zinda
         *  Date Updated: 10.29.18
         *  Description:  Top-level method for adding user-uploaded accession numbers
         *                to the project database.
         *  Parameters:   int id - Project ID, passed through hidden form in ProjectDashboard.cshtml,
         *                         included so that the accession numbers are added to the correct project.
         */
        // POST: /Api/FileController/Upload
        [HttpPost]
        public async Task<IHttpActionResult> Upload(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                var file = provider.FileData[0];

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(file.LocalFileName);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                string hasHeader = provider.FormData.GetValues("hasHeader")[0];                
                int startingIndex = hasHeader.Contains("no") ? 1 : 2;

                List<string> accessions = new List<string>();
                for (int i = startingIndex; i <= rowCount; i++)
                {
                    if (xlRange.Cells[i, 1] != null && xlRange.Cells[i, 1].Value2 != null)
                        accessions.Add(xlRange.Cells[i, 1].Value2.ToString());
                }

                return Ok(accessions);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
