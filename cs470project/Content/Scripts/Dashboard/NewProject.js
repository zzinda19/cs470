/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    12.6.18
*  Description:     Initializes the new project form and processes user submission.
*/
var NewProjectForm = {
    Initialize: function () {
        $("#newProject").validate({
            submitHandler: function () {
                // Initializes a researchProjectDto and assigns form values accordingly.
                var researchProjectDto = {
                    ProjectName: $("#name").val(),
                    ProjectDescription: $("#description").val()
                };

                // Submits form data to create a new research project.
                $.ajax({
                    url: "/api/researchProjects/",
                    method: "post",
                    data: researchProjectDto,
                    success: function (data) {
                        toastr.success("Project successfully created.");
                        // Redirect user to the new project's dashboard.
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

$(document).ready(NewProjectForm.Initialize());