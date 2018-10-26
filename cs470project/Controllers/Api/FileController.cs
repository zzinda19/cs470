using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;

namespace cs470project.Controllers.Api
{
    public class FileController : ApiController
    {

        // POST: /Api/FileController/Upload
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
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

                var fileName = file.Headers.ContentDisposition.FileName;

                return Ok(fileName);
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
