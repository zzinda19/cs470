/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    12.6.18
*  Description:     Populates the project dashboard home page with the appropriate project information.
*/
var ProjectDashboard = {
    Initialize: function () {
        var id = $("#projectID").val();

        $.ajax({
            url: "/Api/ResearchProjects/" + id,
            success: function (data) {
                $("#header").text(data.projectName);
                $("#description").text(data.projectDescription);
                $("#insertDate").text(data.insertDate);
            },
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
            }
        });
    }
};

$(document).ready(function () {
    ProjectDashboard.Initialize();
});