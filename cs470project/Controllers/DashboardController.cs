using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using cs470project.Dtos;
using cs470project.Models;
using cs470project.ViewModels;

namespace cs470project.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dashboard/ProjectDashboard/1
        public ActionResult ProjectDashboard(int id)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProject = context.ResearchProjects
                    .SingleOrDefault(p => p.ProjectID == id);

                if (researchProject == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new DashboardViewModel
                {
                    ProjectId = researchProject.ProjectID
                };

                return View(viewModel);
            }
                
        }

        // GET: Dashboard/NewProject
        public ActionResult NewProject()
        {
            return View();
        }

        // GET: Dashboard/EditProject/1
        public ActionResult EditProject(int id)
        {
            using (var context = new CCFDataEntities())
            {
                var researchProject = context.ResearchProjects
                    .SingleOrDefault(p => p.ProjectID == id);

                if (researchProject == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new DashboardViewModel
                {
                    ProjectId = researchProject.ProjectID
                };

                return View(viewModel);
            }
        }

        /**
         *  Author: Zak Zinda
         *  Date Updated: 11.30.18
         *  Description: Returns a list of global users in the database. Used by the 
         *               Bloodhound plug-in in Users.js for auto-filling usernames in
         *               the add user form.
         *  Parameters:  int projectId - the id of the project to download accessions/MRNs from.
         *               int downloadType - the type of files the user wishes to download--see
         *               DownloadType.cs for the three download types:
         *                  1 = AccessionsOnly
         *                  2 = MRNsOnly
         *                  3 = Both
         */
        // POST: Dashboard/DownloadKeyPairs
        [HttpPost]
        public ActionResult DownloadKeyPairs(int projectId, int downloadType)
        {
            // Create new stringbuilder and empty file name.
            var sb = new System.Text.StringBuilder();
            var fileName = "KeyPairs.csv";

            using (var context = new CCFDataEntities())
            {
                var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == projectId);

                // First check if the associated project exists.
                if (researchProjectInDb == null)
                {
                    return HttpNotFound();
                }

                // Return all ResearchProjectAccession objects associated with that project.
                var keyPairs = context.ResearchProjectAccessions
                            .Where(p => p.ProjectID == projectId)
                            .ToList()
                            .Select(Mapper.Map<ResearchProjectAccession, KeyPairDto>);

                // Check if any key pairs exist. If not, skip downloadType check.
                if (keyPairs.Count() == 0)
                {
                    sb.Append("This project does not yet have any accession numbers uploaded.\r\n");
                }
                else
                {
                    // Checks downloadType.
                    // DownloadType.AccessionOnly = 1
                    // DownloadType.MRNOnly = 2
                    // DownloadType.Both = 3
                    switch (downloadType)
                    {
                        case DownloadType.AccessionOnly:
                            // Set appropriate file name.
                            fileName = "AccessionKeyPairs.csv";

                            // Add only accession numbers associated with that project to the file.
                            sb.Append("Accession,Randomized Accession\r\n");
                            foreach (var keyPair in keyPairs)
                            {
                                sb.AppendFormat("=\"{0}\",", keyPair.Accession.ToString());
                                sb.AppendFormat("=\"{0}\"\r\n", keyPair.AccessionGuid.ToString());
                            }
                            break;
                        case DownloadType.MRNOnly:
                            // Set appropriate file name.
                            fileName = "MRNKeyPairs.csv";
                            
                            // Add only MRNs associated with that project to the file.
                            sb.Append("MRN,Randomized MRN\r\n");
                            foreach (var keyPair in keyPairs)
                            {
                                sb.AppendFormat("=\"{0}\",", keyPair.MRN.ToString());
                                sb.AppendFormat("=\"{0}\"\r\n", keyPair.MRNGuid.ToString());
                            }
                           
                            break;
                        case DownloadType.Both:
                            // Set appropriate file name.
                            fileName = "AccessionAndMRNKeyPairs.csv";
                            
                            // Add both accessions and MRNs to the file.
                            sb.Append("Accession,Randomized Accession,MRN,Randomized MRN\r\n");
                            foreach (var keyPair in keyPairs)
                            {
                                sb.AppendFormat("=\"{0}\",", keyPair.Accession.ToString());
                                sb.AppendFormat("=\"{0}\",", keyPair.AccessionGuid.ToString());
                                sb.AppendFormat("=\"{0}\",", keyPair.MRN.ToString());
                                sb.AppendFormat("=\"{0}\"\r\n", keyPair.MRNGuid.ToString());
                            }
                            
                            break;
                        default:
                            break;
                    }
                }
                
                // Convert stringbuilder into a file string.
                string file = sb.ToString();
                // Return file as a csv.
                return File(new System.Text.UTF8Encoding().GetBytes(file), "text/csv", fileName);
            }
        }
    }
}