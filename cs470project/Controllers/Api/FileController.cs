using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace cs470project.Controllers.Api
{
    public class FileController : ApiController
    {

        // POST: /Api/FileController/Upload
        [HttpPost]
        public IHttpActionResult Upload(HttpPostedFileBase File)
        {
            if (File.ContentLength == 0)
            {
                return BadRequest();
            }

            return NotFound();
        }
    }
}
