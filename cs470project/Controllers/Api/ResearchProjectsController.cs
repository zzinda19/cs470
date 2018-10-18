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
    public class ResearchProjectsController : ApiController
    {
        // GET /Api/ResearchProjects
        /*
         *  Author: Zak Zinda
         *  Description: 
         *  
         */
        public IHttpActionResult GetResearchProjects()
        {
            using (var context = new CCFDataEntities())
            {
                var researchProjects = context.ResearchProjects
                    .ToList()
                    .Select(Mapper.Map<ResearchProject, ResearchProjectDto>);

                return Ok(researchProjects);
            }
        }

        // GET /Api/ResearchProjects/1
        public IHttpActionResult GetResearchProject(int id)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProject = context.ResearchProjects.SingleOrDefault(p => p.ProjectID == id);

                if (researchProject == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<ResearchProjectDto>(researchProject));
            }
        }

        // POST: /Api/ResearchProjects/
        [HttpPost]
        public IHttpActionResult CreateResearchProject(ResearchProjectDto researchProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var researchProject = Mapper.Map<ResearchProjectDto, ResearchProject>(researchProjectDto);
            researchProject.InsertDate = DateTime.Now;

            using (var context = new CCFDataEntities())
            {
                context.ResearchProjects.Add(researchProject);
                context.SaveChanges();
            }

            researchProjectDto.ProjectID = researchProject.ProjectID;
            researchProjectDto.InsertDate = researchProject.InsertDate.ToShortDateString();

            return Created(new Uri(Request.RequestUri + "/" + researchProject.ProjectID), researchProjectDto);
        }

        // PUT: /Api/ResearchProjects/1
        [HttpPut]
        public IHttpActionResult Update(int id, ResearchProjectDto researchProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            using (var context = new CCFDataEntities())
            {
                var researchProjectInDb = context.ResearchProjects.SingleOrDefault(p => p.ProjectID == id);

                if (researchProjectInDb == null)
                {
                    return NotFound();
                }

                Mapper.Map(researchProjectDto, researchProjectInDb);
                Mapper.Map(researchProjectInDb, researchProjectDto);

                context.SaveChanges();
            }

            return Ok(researchProjectDto);
        }
    }
}
