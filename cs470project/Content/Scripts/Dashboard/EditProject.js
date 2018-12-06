/**
*  Author:          Zak Zinda
*  Updated By:      Landry Snead
*  Date Updated:    12.4.18
*  Description:     Initializes the edit project form and processes user submission.
*/
var EditProjectForm = {

    Initialize: function () {
        var projectId = $("#projectID").val();
        EditProjectForm.PopulateForm(projectId);
        EditProjectForm.SetupValidationAndSubmitHandler(projectId);
    },

    // Populates form with the existing research project form values.
    PopulateForm: function (id) {
        $.ajax({
            url: "/Api/ResearchProjects/" + id,
            success: function (data) {
                $("#name").val(data.projectName);
                $("#description").val(data.projectDescription);
            },
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.responseJSON.message);
            }
        });
    },

    // Submits form data to update the current research project.
    SetupValidationAndSubmitHandler: function (id) {
        $("#editProject").validate({
            submitHandler: function () {
                // Initializes a researchProjectDto and assigns form values accordingly.
                var researchProjectDto = {
                    ProjectName: $("#name").val(),
                    ProjectDescription: $("#description").val()
                };

                $.ajax({
                    url: "/Api/ResearchProjects/" + id,
                    method: "put",
                    data: researchProjectDto,
                    success: function (data) {
                        toastr.success("Project successfully updated.");
                        // Redirect user back to the project's dashboard.
                        var url = "/Dashboard/ProjectDashboard/" + data.projectID;
                        $(location).attr('href', url);
                    },
                    error: function (xhr) {
                        toastr.error("An error occured: " + xhr.responseJSON.message);
                    }
                });
            }
        });
    }
};

$(document).ready(EditProjectForm.Initialize());