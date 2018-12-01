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
    }
}