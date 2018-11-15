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

namespace cs470project.Controllers.Api
{
    public class FileController : ApiController
    {

        /**
         *  Author: Zak Zinda
         *  Date Updated: 10.29.18
         *  Description: Top-level method for adding user-uploaded accession numbers
         *               to the project database.
         *  Parameters: int id - Project ID, passed through hidden form in ProjectDashboard.cshtml,
         *                       included so that the accession numbers are added to the correct project.
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

                using (var context = new CCFDataEntities())
                {
                    var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == id);

                    if (researchProjectInDb == null)
                    {
                        return BadRequest("The selected research project does not exist.");
                    }

                    foreach(var accessionNumber in accessionNumbers)
                    {
                        var accession = new ResearchProjectAccession()
                        {
                            ResearchProject = researchProjectInDb,
                            Accession = Convert.ToInt32(accessionNumber),
                            AlternateGUID = Guid.NewGuid()
                        };
                        context.ResearchProjectAccessions.Add(accession);
                        context.SaveChanges();
                    }
                }

                return Ok();
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
                        foreach(var keyPair in accessionKeyPairs)
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
