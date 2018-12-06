/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    12.6.18
*  Description:     Initializes the index data table with all current research projects.
*/
var Index = {
    SetupIndexDataTable: function () {
        $("#researchProjects").DataTable({
            ajax: {
                url: "/Api/ResearchProjects",
                dataSrc: ""
            },
            columns: [
                {
                    data: "projectName",
                    render: function (data, type, researchProject) {
                        // Returns a link to the selected project's dashboard page.
                        return "<a href='/Dashboard/ProjectDashboard/" + researchProject.projectID + "'>" + researchProject.projectName + "</a>";
                    }
                },
                {
                    data: "projectDescription"
                },
                {
                    data: "insertDate"
                }
            ]
        });
    }
}

$(document).ready(function () {
    Index.SetupIndexDataTable();
});
