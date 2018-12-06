using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs470project.Dtos
{
    public class AccessionDto
    {
        [Required]
        public int Accession { get; set; }
        [Required]
        public Guid AccessionGuid { get; set; }

    }
}