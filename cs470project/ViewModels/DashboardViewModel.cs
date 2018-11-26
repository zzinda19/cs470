using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using cs470project.Models;

namespace cs470project.ViewModels
{
    public class DashboardViewModel
    {
        [Required]
        public int ProjectId { get; set; }
        [Display(Name = "Which set of key pairs would you like to download?")]
        public int DownloadType { get; set; }
    }
}