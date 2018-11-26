using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs470project.Dtos
{
    public class MRNDto
    {
        [Required]
        [StringLength(64)]
        public string MRN { get; set; }
        [Required]
        public Guid AlternateGuid { get; set; }
    }
}