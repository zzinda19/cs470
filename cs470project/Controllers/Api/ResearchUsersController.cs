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
    public class ResearchUsersController : ApiController
    {
        /**
         *  Author: Zak Zinda
         *  Date Updated: 11.15.18
         *  Description: Returns a list of the users associated with a certain research project.
         *  Parameters: int id - Project ID
         */
        // GET: /Api/ResearchUsers/1
        public IHttpActionResult GetResearchProjectUsers(int id)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProjectInDb = context.ResearchProjects.SingleOrDefault(p => p.ProjectID == id);

                if (researchProjectInDb == null)
                {
                    return BadRequest("The specified research project is invalid.");
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
         * Author: Zak Zinda
         * Date Updated: 11.29.18
         * Description: Adds a user to a selected research project.
         */
        // POST: /Api/ResearchUsers/1
        [Route("Api/ResearchUsers/Add")]
        [HttpPost]
        public IHttpActionResult AddResearchUserToProject(ResearchProjectUserDto researchProjectUserDto)
        {
            var projectId = researchProjectUserDto.ProjectId;
            var userId = researchProjectUserDto.UserId;

            // Ensure data is transmitted correctly.
            if (!ModelState.IsValid)
            {
                return BadRequest("Improper form submission.");
            }

            using (var context = new CCFDataEntities())
            {
                // First check if user is already added to project.
                var researchProjectUserInDb = context.ResearchProjectUsers
                    .SingleOrDefault(u => u.UserID == userId && u.ProjectID == projectId);
                // Prevent adding users twice to the same project.
                if (researchProjectUserInDb != null)
                {
                    return BadRequest("This user is already added to the project.");
                }

                // Next check if the specified research project exists.
                var researchProjectInDb = context.ResearchProjects
                    .SingleOrDefault(p => p.ProjectID == projectId);
                // Prevent adding users to non-existent projects.
                if (researchProjectInDb == null)
                {
                    return BadRequest("The specified research project is invalid.");
                }

                // Next check if the specified user exists in the global user DB.
                var userInDb = context.ResearchUsers
                    .SingleOrDefault(u => u.UserId == userId);
                // Prevent adding non-existent users to a project.
                if (userInDb == null)
                {
                    return BadRequest("The specified user is invalid.");
                }

                // If both the user and project exist and the user is not already added,
                // create a new record for that user within the specified research project.
                var researchProjectUser = new ResearchProjectUser
                {
                    ResearchProject = researchProjectInDb,
                    ResearchUser = userInDb,
                    Admin = researchProjectUserDto.Admin
                };

                context.ResearchProjectUsers.Add(researchProjectUser);
                context.SaveChanges();

                // Rebind the updated researchProjectUserDto.
                Mapper.Map(researchProjectUser, researchProjectUserDto);
                
                return Created(new Uri(Request.RequestUri + "/" + researchProjectUser.ProjectID), researchProjectUserDto);
            }
        }

        /**
         *  Author: Zak Zinda
         *  Date Updated: 11.28.18
         *  Description: Removes a selected user from a selected research project.
         */
        // DELETE /Api/ResearchUsers/1/1
        [Route("Api/ResearchUsers/{projectId}/{userId}")]
        [HttpDelete]
        public IHttpActionResult DeleteResearchProjectUser(int projectId, int userId)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProjectUserInDb = context.ResearchProjectUsers
                    .SingleOrDefault(p => p.ProjectID == projectId && p.UserID == userId);

                if (researchProjectUserInDb == null)
                {
                    return BadRequest("The specified user is not currently added on this project.");
                }

                var username = researchProjectUserInDb.ResearchUser.Username;

                context.ResearchProjectUsers.Remove(researchProjectUserInDb);
                context.SaveChanges();

                return Ok(username + " has been removed from this project.");
            }
        }
    }
}
