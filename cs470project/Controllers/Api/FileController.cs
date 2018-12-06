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

                List<RejectedAccessionDto> rejectedAccessions = new List<RejectedAccessionDto>();

                using (var context = new CCFDataEntities())
                {
                    var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == id);

                    if (researchProjectInDb == null)
                    {
                        return BadRequest("The selected research project does not exist.");
                    }

                    rejectedAccessions = ValidateUploadedAccessions(accessionNumbers, context, researchProjectInDb);
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
        *  Updated By:   Zak Zinda
        *  Date Updated: 11.10.18
        *  Description:  This checks each uploaded accession number to see if it is valid. 
        *                  Step 1: is it in the correct format (an integer value)?
        *                  Step 2: is it already included in the project?
        *                  Step 3: does the accession exist? --not implemented 
        *                  Then it adds the accession to the project.
        *  Parameters:   List<string> accessions: A string list of accession numbers from the uploaded file.
        *                CCFDataEntities context: the database connection
        *                ResearchProject researchProjectInDb: The research project object
        */
        [NonAction]
        public List<RejectedAccessionDto> ValidateUploadedAccessions(List<string> accessions, CCFDataEntities context, ResearchProject researchProjectInDb)
        {
            List<RejectedAccessionDto> rejectedAccessions = new List<RejectedAccessionDto>();

            var projectId = researchProjectInDb.ProjectID;

            foreach (var accessionNumber in accessions)
            {
                RejectedAccessionDto rejectedAccession;
                // Ensure the accession number is an integer.
                if (!accessionNumber.All(char.IsDigit))
                {
                    rejectedAccession = new RejectedAccessionDto
                    {
                        Accession = accessionNumber,
                        Reason = "Accession must be an integer."
                    };
                    rejectedAccessions.Add(rejectedAccession);
                    continue;
                }

                var convertedAccessionNumber = Convert.ToInt32(accessionNumber.Trim());

                var researchAccessionInDb = context.ResearchProjectAccessions
                    .SingleOrDefault(a => a.ProjectID == projectId && a.Accession == convertedAccessionNumber);

                // Ensure the accession number has not already been added.
                if (researchAccessionInDb != null)
                {
                    rejectedAccession = new RejectedAccessionDto
                    {
                        Accession = accessionNumber,
                        Reason = "Accession has already been added to the project."
                    };
                    rejectedAccessions.Add(rejectedAccession);
                    continue;
                }

                var globalAccessionInDb = context.GlobalAccessions
                    .SingleOrDefault(a => a.Accession == convertedAccessionNumber);

                // Ensure the accession number exists globally.
                if (globalAccessionInDb == null)
                {
                    rejectedAccession = new RejectedAccessionDto
                    {
                        Accession = accessionNumber,
                        Reason = "Accession does not exist in the global database."
                    };
                    rejectedAccessions.Add(rejectedAccession);
                    continue;
                }

                //accession passed validation, create an accession object to add to the database
                var accession = new ResearchProjectAccession()
                {
                    ResearchProject = researchProjectInDb,
                    Accession = globalAccessionInDb.Accession,
                    AccessionGUID = Guid.NewGuid(), //deidentified accession number
                    MRN = globalAccessionInDb.MRN,
                    MRNGUID = Guid.NewGuid()
                };

                context.ResearchProjectAccessions.Add(accession);
                context.SaveChanges();
            }

            return rejectedAccessions;
        }
    }
}   
