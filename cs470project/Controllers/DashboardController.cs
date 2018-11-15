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

                var viewModel = new ProjectApiViewModel
                {
                    ProjectID = researchProject.ProjectID
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

                var viewModel = new ProjectApiViewModel
                {
                    ProjectID = researchProject.ProjectID
                };

                return View(viewModel);
            }
        }
   

        // POST: Dashboard/Upload
        [HttpPost]
        public ActionResult Upload(ProjectApiViewModel viewModel, HttpPostedFileBase File)
        {
            var id = viewModel.ProjectID;

            if (Request.Files[0] == null)
            {
                return Content("No file found.");
            }

            return Content(Request.Files[0].FileName);

            //return RedirectToAction("ProjectDashboard/" + id, "Dashboard#uploadMenu");
        }

        // POST: Dashboard/Download
        [HttpPost]
        public FileContentResult Download(DownloadRequestDto downloadRequest)
        {
            var sb = new System.Text.StringBuilder();
            var fileName = "";

            using (var context = new CCFDataEntities())
            {
                var id = downloadRequest.ProjectId;

                var researchProjectInDb = context.ResearchProjects.Single(p => p.ProjectID == id);

                if (researchProjectInDb == null)
                {
                    
                }

                var downloadType = downloadRequest.DownloadType;

                switch (downloadType)
                {
                    case DownloadType.AccessionOnly:
                        fileName = "AccessionKeyPairs.csv";
                        var accessionKeyPairs = context.ResearchProjectAccessions
                            .Where(p => p.ProjectID == id)
                            .ToList()
                            .Select(Mapper.Map<ResearchProjectAccession, AccessionDto>);

                        sb.Append("Accession,AlternateGuid\r\n");
                        foreach (var keyPair in accessionKeyPairs)
                        {
                            sb.AppendFormat("=\"{0}\",", keyPair.Accession.ToString());
                            sb.AppendFormat("=\"{0}\"\r\n", keyPair.AlternateGuid.ToString());
                        }
                        break;
                    case DownloadType.MRNOnly:
                        fileName = "MRNKeyPairs.csv";

                        break;
                    case DownloadType.Both:
                        fileName = "AccessionAndMRNKeyPairs.csv";

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