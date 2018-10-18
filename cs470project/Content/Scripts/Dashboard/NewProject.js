var layout = {
    Initialize: function () {
        $("#newProject").validate({
            submitHandler: function () {
                var vm = {};
                vm.ProjectName = $("#name").val();
                vm.ProjectDescription = $("#description").val();

                $.ajax({
                    url: "/api/researchProjects/",
                    method: "post",
                    data: vm,
                    success: function (data) {
                        toastr.success("Project successfully created.");
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

$(document).ready(layout.Initialize());