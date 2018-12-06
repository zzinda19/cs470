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

        /**
         *  Author:         Zak Zinda
         *  Updated By:     
         *  Date Updated:   12.6.18
         *  Description:    Returns a list of existing research projects.
         */
        // GET /Api/ResearchProjects
        [HttpGet]
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

        /**
         *  Author:         Zak Zinda
         *  Updated By:     
         *  Date Updated:   12.6.18
         *  Description:    Return the project information for a selected research project.
         *  Parameters:     int id - The id of the project to query.
         */
        // GET /Api/ResearchProjects/1
        public IHttpActionResult GetResearchProject(int id)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProject = context.ResearchProjects.SingleOrDefault(p => p.ProjectID == id);

                if (researchProject == null)
                {
                    return BadRequest("The selected research project could not be found.");
                }

                return Ok(Mapper.Map<ResearchProjectDto>(researchProject));
            }
        }

        /**
         *  Author:         Zak Zinda
         *  Updated By:     
         *  Date Updated:   12.6.18
         *  Description:    Creates a new research project using user-submitted form data.
         *  Parameters:     ResearchProjectDto researchProjectDto - a data transfer object
         *                  containing the users form data.
         */
        // POST: /Api/ResearchProjects/
        [HttpPost]
        public IHttpActionResult CreateResearchProject(ResearchProjectDto researchProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid form data.");
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

        /**
         *  Author:         Zak Zinda
         *  Updated By:     
         *  Date Updated:   12.6.18
         *  Description:    Updates an existing research project using user-submitted form data.
         *  Parameters:     int id - the id of the research project the user wishes to update.
         *                  ResearchProjectDto researchProjectDto - a data transfer object
         *                  containing the users form data.
         */
        // PUT: /Api/ResearchProjects/1
        [HttpPut]
        public IHttpActionResult Update(int id, ResearchProjectDto researchProjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid form data.");
            }

            using (var context = new CCFDataEntities())
            {
                var researchProjectInDb = context.ResearchProjects.SingleOrDefault(p => p.ProjectID == id);

                if (researchProjectInDb == null)
                {
                    return BadRequest("The selected research project could not be found.");
                }

                Mapper.Map(researchProjectDto, researchProjectInDb);
                Mapper.Map(researchProjectInDb, researchProjectDto);

                context.SaveChanges();
            }

            return Ok(researchProjectDto);
        }
    }
}
