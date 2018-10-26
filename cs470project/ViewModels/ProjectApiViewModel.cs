using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cs470project.ViewModels
{
    public class ProjectApiViewModel
    {
        public int ProjectID { get; set; }
        public string UploadErrorMessage { get; set; }
        public List<string> RejectedAccessions { get; set; }
    }
}