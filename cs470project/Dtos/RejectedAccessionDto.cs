using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs470project.Dtos
{
    public class RejectedAccessionDto
    {
        [Required]
        [StringLength(255)]
        public string Accession { get; set; }
        [Required]
        [StringLength(255)]
        public string Reason { get; set; }
    }
}