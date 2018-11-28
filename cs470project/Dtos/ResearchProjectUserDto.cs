using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs470project.Dtos
{
    public class ResearchProjectUserDto
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public ResearchUserDto ResearchUser { get; set; }

        [Required]
        public bool Admin { get; set; }
    }
}