$(document).ready(function () {
    var id = $("#projectID").val();
    var table = $("#researchProjectUsers").DataTable({
        ajax: {
            url: "/Api/Users/" + id,
            dataSrc: "",
            error: function (xhr) {
                console.log(xhr);
                console.log(xhr.responseText);
                console.log(xhr.statusCode);
                console.log(xhr.statusText);
            }
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
});