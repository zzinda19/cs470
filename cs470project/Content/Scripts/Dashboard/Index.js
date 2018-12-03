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
