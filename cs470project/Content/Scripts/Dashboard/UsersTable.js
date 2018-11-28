$(document).ready(function () {
    var projectId = $("#projectID").val();
    var table = $("#researchProjectUsers").DataTable({
        ajax: {
            url: "/Api/Users/" + projectId,
            dataSrc: "",
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
            }
        },
        columns: [
            {
                data: "researchUser.username"
            },
            {
                data: "admin",
                render: function (data) {
                    if (data == true) {
                        return "Admin";
                    } else {
                        return "User";
                    }
                }
            },
            {
                data: "userID",
                render: function (data) {
                    return "<button class='btn-link js-delete' data-user-id=" + data + ">Remove</button>";
                }
            }
        ]
    });

    $("#researchProjectUsers").on("click", ".js-delete", function () {
        var button = $(this);
        bootbox.confirm("Are you sure you want to remove this user from the project?", function (result) {
            if (result) {
                $.ajax({
                    url: "/Api/Users/" + projectId + "/" + button.attr("data-user-id"),
                    method: "DELETE",
                    success: function () {
                        table.row(button.parents("tr")).remove().draw();
                    }
                });
            }
        });
    });
});