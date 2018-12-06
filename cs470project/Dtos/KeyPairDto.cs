using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs470project.Dtos
{
    public class KeyPairDto
    {
        public int Accession { get; set; }
        public Guid AccessionGuid { get; set; }
        [StringLength(64)]
        public string MRN { get; set; }
        public Guid MRNGuid { get; set; }
    }
}