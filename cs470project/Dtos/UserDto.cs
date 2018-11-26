using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using cs470project.Models;

namespace cs470project.Dtos
{
    public class UserDto
    {
        [Required]
        public int UserID { get; set; }

        public ResearchUser ResearchUser { get; set; }

        [Required]
        public byte Admin { get; set; }
    }
}