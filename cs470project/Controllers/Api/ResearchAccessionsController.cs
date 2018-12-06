using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cs470project.Models;
using cs470project.Dtos;
using AutoMapper;

namespace cs470project.Controllers.Api
{
    public class ResearchAccessionsController : ApiController
    {
        // GET /Api/ResearchProjects
        public IHttpActionResult GetResearchAccessions()
        {
            using (var context = new CCFDataEntities())
            {
                var researchAccessions = context.ResearchProjectAccessions
                    .ToList()
                    .Select(Mapper.Map<ResearchProjectAccession, AccessionDto>);

                return Ok(researchAccessions);
            }
        }

        // GET /Api/ResearchProjects/1
        public IHttpActionResult GetResearchAccession(int id)
        {
            using (var context = new CCFDataEntities())
            {
                var researchAccession = context.ResearchProjectAccessions.SingleOrDefault(p => p.Accession == id);

                if (researchAccession == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<AccessionDto>(researchAccession));
            }
        }
    }
}