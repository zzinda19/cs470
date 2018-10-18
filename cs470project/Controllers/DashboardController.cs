using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Upload(DashboardViewModel viewModel)
        {
            var id = viewModel.ResearchProject.ProjectID;



            return RedirectToAction("ProjectDashboard/" + id, "Dashboard");
        }

        // POST: Dashboard/Download
        [HttpPost]
        public ActionResult Download(DashboardViewModel viewModel)
        {
            var id = viewModel.ResearchProject.ProjectID;

            switch (viewModel.DownloadType)
            {
                case DownloadType.AccessionOnly:
                    // Create Accessions Only File and Download
                    return RedirectToAction("ProjectDashboard/" + id, "Dashboard");
                case DownloadType.MRNOnly:
                    // Create MRN Only File and Download
                    return RedirectToAction("ProjectDashboard/" + id, "Dashboard");
                case DownloadType.Both:
                    // Create Both File and Download
                    return RedirectToAction("ProjectDashboard/" + id, "Dashboard");
                default:
                    return HttpNotFound();
            }   
        }
    }
}