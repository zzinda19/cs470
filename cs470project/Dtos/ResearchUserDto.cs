using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using cs470project.Models;

namespace cs470project.Dtos
{
    public class ResearchUserDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }
    }
}