using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs470project.Dtos
{
    public class ResearchAccessionDto
    {
        public int ProjectID { get; set; }

        [Required]
        public int Accession { get; set; }

    }
}