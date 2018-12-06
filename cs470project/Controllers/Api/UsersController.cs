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
        /**
         *  Author: Zak Zinda
         *  Date Updated: 11.15.18
         *  Description: Returns a list of global users in the database. Used by the 
         *               Bloodhound plug-in in Users.js for auto-filling usernames in
         *               the add user form.
         *  Parameters:  string query - Used by Bloodhound, as the user starts typing in
         *               a username the API queries only global users where the query string
         *               matches part of the username.
         */
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
