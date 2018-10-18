using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using cs470project.Models;

namespace cs470project.ViewModels
{
    public class ProjectFormViewModel
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }

        public string Title
        {
            get
            {
                return ProjectID != 0 ? "Edit Project" : "New Project";
            }
        }

        public ProjectFormViewModel()
        {
            ProjectID = 0;
        }

        public ProjectFormViewModel(ResearchProject researchProject)
        {
            ProjectID = researchProject.ProjectID;
            ProjectName = researchProject.ProjectName;
            ProjectDescription = researchProject.ProjectDescription;
        }
    }
}