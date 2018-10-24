using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cs470project.Models;

namespace cs470project.ViewModels
{
    public class DashboardViewModel
    {
        public ResearchProject ResearchProject { get; set; }
        public ResearchProjectAccession ResearchAccession { get; set; }
        public int DownloadType { get; set; }
    }
}