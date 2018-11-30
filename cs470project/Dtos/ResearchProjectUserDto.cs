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
        public int ProjectId { get; set; }

        [Required]
        public int UserId { get; set; }

        public ResearchUserDto ResearchUser { get; set; }

        [Required]
        public bool Admin { get; set; }
    }
}