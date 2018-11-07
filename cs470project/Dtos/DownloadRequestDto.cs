using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cs470project.Dtos
{
    public class DownloadRequestDto
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int DownloadType { get; set; }
    }
}