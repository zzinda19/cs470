var Layout = {
    Initialize: function () {
        Layout.PopulateDashboard();
        Layout.SetupValidationAndSubmitHandler();
    },

    PopulateDashboard: function () {
        var id = $("#projectID").val();
        $.ajax({
            url: "/Api/ResearchProjects/" + id,
            success: function (data) {
                $("#name").val(data.projectName);
                $("#description").val(data.projectDescription);
            },
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
            }
        });
    },

    SetupValidationAndSubmitHandler: function () {
        $("#editProject").validate({
            submitHandler: function () {
                var id = $("#projectID").val();

                var vm = {};
                vm.ProjectName = $("#name").val();
                vm.ProjectDescription = $("#description").val();

                $.ajax({
                    url: "/Api/ResearchProjects/" + id,
                    method: "put",
                    data: vm,
                    success: function (data) {
                        toastr.success("Project successfully updated.");
                        console.log(data);
                        var url = "/Dashboard/ProjectDashboard/" + data.projectID;
                        $(location).attr('href', url);
                    },
                    error: function (xhr) {
                        toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                    }
                });
            }
        });
    }
};

$(document).ready(Layout.Initialize());