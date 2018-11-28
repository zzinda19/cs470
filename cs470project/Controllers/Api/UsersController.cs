using cs470project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using cs470project.Dtos;

namespace cs470project.Controllers.Api
{
    public class UsersController : ApiController
    {
        /**
         *  Author: Zak Zinda
         *  Date Updated: 11.15.18
         *  Description: Returns a list of the users associated with a certain research project.
         *  Parameters: int id - Project ID
         */
        // GET: /Api/Users/1
        public IHttpActionResult GetResearchProjectUsers(int id)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProjectInDb = context.ResearchProjects.SingleOrDefault(p => p.ProjectID == id);

                if (researchProjectInDb == null)
                {
                    return BadRequest();
                }

                var researchProjectUsers = context.ResearchProjectUsers
                    .Where(p => p.ProjectID == id)
                    .Include(p => p.ResearchUser)
                    .ToList()
                    .Select(Mapper.Map<ResearchProjectUser, ResearchProjectUserDto>);

                return Ok(researchProjectUsers);
            }
        }

        /**
         *  Author: Zak Zinda
         *  Date Updated: 11.28.18
         *  Description: Removes a selected user from a selected research project.
         */
        // DELETE /Api/Users/1
        [Route("Api/Users/{projectId}/{userId}")]
        [HttpDelete]
        public IHttpActionResult DeleteResearchProjectUser(int projectId, int userId)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProjectUserInDb = context.ResearchProjectUsers
                    .SingleOrDefault(p => p.ProjectID == projectId && p.UserID == userId);

                if (researchProjectUserInDb == null)
                {
                    return NotFound();
                }

                context.ResearchProjectUsers.Remove(researchProjectUserInDb);
                context.SaveChanges();

                return Ok();

            }
        }
    }
}
