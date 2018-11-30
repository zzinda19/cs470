using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cs470project.Models;
using cs470project.Dtos;
using AutoMapper;

namespace cs470project.Controllers.Api
{
    public class UsersController : ApiController
    {
        // GET: /Api/Users
        public IHttpActionResult GetUsers(string query = null)
        {
            using (var context = new CCFDataEntities())
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    var usersQuery = context.ResearchUsers
                        .Where(u => u.Username.Contains(query))
                        .ToList()
                        .Select(Mapper.Map<ResearchUser, ResearchUserDto>);

                    return Ok(usersQuery);
                }   
                else
                {
                    var users = context.ResearchUsers
                    .ToList()
                    .Select(Mapper.Map<ResearchUser, ResearchUserDto>);

                    return Ok(users);
                }
            }
        }
    }
}
