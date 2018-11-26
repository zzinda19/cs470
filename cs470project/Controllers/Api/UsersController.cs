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
                var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == id);

                if (researchProjectInDb == null)
                {
                    return BadRequest("The selected research project does not exist.");
                }

                var researchUsers = context.ResearchUsers.ToList();

                return Ok(researchUsers);
            }
        }







    }
}
