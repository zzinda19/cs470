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
                    .Include(p => p.ResearchProjectAccessions)
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

        // POST: Dashboard/DownloadKeyPairs
        [HttpPost]
        public ActionResult DownloadKeyPairs(int projectId, int downloadType)
        {
            var sb = new System.Text.StringBuilder();
            var fileName = "";

            using (var context = new CCFDataEntities())
            {
                var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == projectId);

                if (researchProjectInDb == null)
                {
                    return HttpNotFound();
                }

                switch (downloadType)
                {
                    case DownloadType.AccessionOnly:
                        fileName = "AccessionKeyPairs.csv";
                        var accessionKeyPairs = context.ResearchProjectAccessions
                            .Where(p => p.ProjectID == projectId)
                            .ToList()
                            .Select(Mapper.Map<ResearchProjectAccession, KeyPairDto>);

                        if (accessionKeyPairs.Count() != 0)
                        {
                            sb.Append("Accession,Randomized Accession\r\n");
                            foreach (var keyPair in accessionKeyPairs)
                            {
                                sb.AppendFormat("=\"{0}\",", keyPair.Accession.ToString());
                                sb.AppendFormat("=\"{0}\"\r\n", keyPair.AccessionGuid.ToString());
                            }
                        } 
                        else
                        {
                            sb.Append("This project does not yet have any accession numbers uploaded.\r\n");
                        }
                        break;
                    case DownloadType.MRNOnly:
                        fileName = "MRNKeyPairs.csv";
                        var MRNKeyPairs = context.ResearchProjectAccessions
                            .Where(p => p.ProjectID == projectId)
                            .ToList()
                            .Select(Mapper.Map<ResearchProjectAccession, KeyPairDto>);
                        if (MRNKeyPairs.Count() != 0)
                        {
                            sb.Append("MRN,Randomized MRN\r\n");
                            foreach (var keyPair in MRNKeyPairs)
                            {
                                sb.AppendFormat("=\"{0}\",", keyPair.MRN.ToString());
                                sb.AppendFormat("=\"{0}\"\r\n", keyPair.MRNGuid.ToString());
                            }
                        }
                        else
                        {
                            sb.Append("This project does not yet have any accession numbers uploaded.\r\n");
                        }
                        break;
                    case DownloadType.Both:
                        fileName = "AccessionAndMRNKeyPairs.csv";
                        var DualKeyPairs = context.ResearchProjectAccessions
                            .Where(p => p.ProjectID == projectId)
                            .ToList()
                            .Select(Mapper.Map<ResearchProjectAccession, KeyPairDto>);
                        if (DualKeyPairs.Count() != 0)
                        {
                            sb.Append("Accession,Randomized Accession,MRN,Randomized MRN\r\n");
                            foreach (var keyPair in DualKeyPairs)
                            {
                                sb.AppendFormat("=\"{0}\",", keyPair.Accession.ToString());
                                sb.AppendFormat("=\"{0}\",", keyPair.AccessionGuid.ToString());
                                sb.AppendFormat("=\"{0}\",", keyPair.MRN.ToString());
                                sb.AppendFormat("=\"{0}\"\r\n", keyPair.MRNGuid.ToString());
                            }
                        }
                        else
                        {
                            sb.Append("This project does not yet have any MRNs uploaded.\r\n");
                        }
                        break;
                    default:
                        break;
                }

                string file = sb.ToString();
                return File(new System.Text.UTF8Encoding().GetBytes(file), "text/csv", fileName);
            }
        }
    }
}