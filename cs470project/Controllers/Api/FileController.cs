using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using cs470project.Models;
using cs470project.Dtos;
using System.Text;
using System.Net.Http.Headers;
using System.Data;

namespace cs470project.Controllers.Api
{
    public class FileController : ApiController
    {
        /**
         *  Author:       Zak Zinda
         *  Date Updated: 10.29.18
         *  Updated By:   Landry Snead
         *  Description:  Top-level method for adding user-uploaded accession numbers
         *                to the project database.
         *  Parameters:   int id - Project ID, passed through hidden form in ProjectDashboard.cshtml,
         *                         included so that the accession numbers are added to the correct project.
         */
        // POST: /Api/File/1
        [HttpPost]
        public async Task<IHttpActionResult> Upload(int id)
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("File improperly formatted.");
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                var file = provider.FileData[0];
                string fileName = file.LocalFileName;

                string hasHeaderFormValue = provider.FormData.GetValues("hasHeader")[0];
                bool hasHeader = hasHeaderFormValue.Contains("yes") ? true : false;

                FileUploadHelper helper = new FileUploadHelper();
                List<string> accessionNumbers = helper.ParseFileToList(fileName, hasHeader);

                DataTable rejectedAccessions = new DataTable();

                using (var context = new CCFDataEntities())
                {
                    var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == id);

                    if (researchProjectInDb == null)
                    {
                        return BadRequest("The selected research project does not exist.");
                    }

                    rejectedAccessions = ValidateUploadedAccessions(accessionNumbers, context, id, researchProjectInDb);
                }

                return Ok(rejectedAccessions);
            }
            catch (InvalidCastException e)
            {
                return InternalServerError(e);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /**
         *  Author:       Landry Snead
         *  Date Updated: 11.10.18
         *  Description:  This checks to see if the accession is already in the project
         *  Parameters:   string accession: the accession number to check (from the uploaded list)
         *                CCFDataEntities context: the database connection
         *                int id: the id of the project to check in
         */
        public Boolean AccessionIsDuplicate(string accession, CCFDataEntities context, int id)
        {
            int accessionAsInt = Convert.ToInt32(accession);
            Boolean accessionExists = context.ResearchProjectAccessions
                                       .Where(a => a.Accession == accessionAsInt && a.ProjectID == id)
                                       .Any();
            return accessionExists;
        }

        /**
         *  Author:       Landry Snead
         *  Date Updated: 11.26.18
         *  Description:  This checks to see if the accession exists. To be implemented...
         *  Parameters:   string accession: the accession number to check (from the uploaded list)
         *                CCFDataEntities context: the database connection
         *                int id: the id of the project to check in
         */
        /*
       public Boolean AccessionExists(string accession, CCFDataEntities context, int id)
       {
           int accessionAsInt = Convert.ToInt32(accession);
           Boolean accessionExists = context.ResearchProjectAccessions
                                      .Where(a => a.Accession == accessionAsInt && a.ProjectID == id)
                                      .Any();
           return accessionExists;
       }
       /*

       /**
        *  Author:       Landry Snead
        *  Date Updated: 11.10.18
        *  Description:  This checks each uploaded accession number to see if it is valid. 
        *                  Step 1: is it in the correct format (an integer value)?
        *                  Step 2: is it already included in the project?
        *                  Step 3: does the accession exist? --not implemented 
        *                  Then is adds the accession to the project.
        *  Parameters:   List<string> accessions: A string list of accession numbers from the uploaded file.
        *                CCFDataEntities context: the database connection
        *                int id: the id of project to add to
        *                ResearchProject researchProjectInDb: The research project object
        */
        [HttpGet]
        public DataTable ValidateUploadedAccessions(List<string> accessions, CCFDataEntities context, int id, ResearchProject researchProjectInDb)
        {
            DataTable rejected = new DataTable();
            rejected.Columns.Add("accession", typeof(string));
            rejected.Columns.Add("reason", typeof(string));

            foreach (var accessionNumber in accessions)
            {
                //check if in the correct format
                if (!accessionNumber.All(char.IsDigit))
                {
                    //accession is not an int, add to rejected
                    rejected.Rows.Add(accessionNumber, "Accession must be an integer");
                    continue;
                }

                //check if duplicate
                else if (AccessionIsDuplicate(accessionNumber, context, id))
                {
                    //accession is already a part of the project, add to rejected
                    rejected.Rows.Add(accessionNumber, "Accession is already included in the project");
                    continue;
                }

                //check if in global database --to be implemented
                //else if (accessionExists(accessionNumber, context,id))
                //{
                //    //accession does not exist in the global database, add to rejected
                //    rejected.Rows.Add(accessionNumber, "Accession does not exist");
                //    continue;
                //}

                //accession passed validation, create an accession object to add to the database
                var accession = new ResearchProjectAccession()
                {
                    ResearchProject = researchProjectInDb,
                    Accession = Convert.ToInt32(accessionNumber.Trim()),
                    AlternateGUID = Guid.NewGuid() //deidentified accession number
                };

                context.ResearchProjectAccessions.Add(accession);
                context.SaveChanges();
            }

            return rejected;
        }

        // GET: /Api/File/1
        [Route("Api/File/Download/")]
        [HttpPost]
        public IHttpActionResult Download(DownloadRequestDto downloadRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Download request is incomplete.");
            }

            var sb = new StringBuilder();
            var fileName = "";

            using (var context = new CCFDataEntities())
            {
                var id = downloadRequest.ProjectId;

                var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == id);

                if (researchProjectInDb == null)
                {
                    return BadRequest("The selected research project does not exist.");
                }

                var downloadType = downloadRequest.DownloadType;

                switch (downloadType)
                {
                    case DownloadType.AccessionOnly:
                        fileName = "AccessionKeyPairs.csv";
                        var accessionKeyPairs = context.ResearchProjectAccessions
                            .Where(p => p.ProjectID == id)
                            .ToList()
                            .Select(Mapper.Map<ResearchProjectAccession, AccessionDto>);

                        sb.Append("Accession,AlternateGuid\r\n");
                        foreach (var keyPair in accessionKeyPairs)
                        {
                            sb.AppendFormat("=\"{0}\",", keyPair.Accession.ToString());
                            sb.AppendFormat("=\"{0}\"\r\n", keyPair.AlternateGuid.ToString());
                        }
                        break;
                    case DownloadType.MRNOnly:
                        fileName = "MRNKeyPairs.csv";

                        break;
                    case DownloadType.Both:
                        fileName = "AccessionAndMRNKeyPairs.csv";

                        break;
                    default:
                        return BadRequest("Improper key-pair type submitted.");
                }

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(sb.ToString())
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

                var response = ResponseMessage(result);

                return response;
            }
        }
    }
}   
