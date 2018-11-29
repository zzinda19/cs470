var projectId = $("#projectID").val();

var userTable = {
    Initialize: function () {
        // Create User DataTable Using Ajax
        var table = $("#researchProjectUsers").DataTable({
            ajax: {
                // Call GetResearchProjectUsers from the Api Users Controller
                url: "/Api/Users/" + projectId,
                dataSrc: "",
                error: function (xhr) {
                    toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                }
            },
            columns: [
                {
                    // Fill column 1 with the users username.
                    data: "researchUser.username"
                },
                {
                    // Fill column 2 with the users admin status.
                    data: "admin",
                    render: function (data) {
                        // Admin is a boolean value. Convert true to Admin, false to User.
                        if (data == true) {
                            return "Admin";
                        } else {
                            return "User";
                        }
                    }
                },
                {
                    // Fill column 3 with a remove link that contains the users id.
                    data: "userID",
                    render: function (data) {
                        return "<button class='btn-link js-delete' data-user-id=" + data + ">Remove</button>";
                    }
                }
            ]
        });

        // Set remove button on click handler.
        table.on("click", ".js-delete", function () {
            var button = $(this);
            // Pull the users id from the remove button's data-user-id attribute.
            var userId = button.attr("data-user-id")
            // If the program user confirms, remove selected user from the research project.
            bootbox.confirm("Are you sure you want to remove this user from the project?", function (result) {
                if (result) {
                    $.ajax({
                        // Call DeleteResearchProjectUser from the Api Users Controller.
                        url: "/Api/Users/" + projectId + "/" + userId,
                        method: "DELETE",
                        success: function () {
                            // If successful delete row and redraw DataTable.
                            table.row(button.parents("tr")).remove().draw();
                        },
                        error: function (xhr) {
                            toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                        }
                    });
                }
            });
        });
    }
}

var addButton = {
    Initialize: function () {
        var button = $("#addUserButton");
        var form = $("#addUserForm");
        button.on("click", function () {
            // Populate modal popup with the hidden addUserForm from _Users.cshtml.
            bootbox.dialog({
                title: "Add User",
                message: form,
                buttons: {
                    cancel: {
                        label: "Cancel",
                        className: "btn btn-default"
                    },
                    ok: {
                        label: "Submit",
                        className: "btn btn-primary",
                        // Callback function for submitting the form.
                        callback: function () {
                            // Pull form values.
                            var vm = {};
                            var username = $("input[name='username']").val();
                            var admin = $("input[name='admin']:checked").val();
                            if (!username) {
                                // If program user does not enter a username return an error.
                                toastr.error("Please input a username.");
                                return false;
                            }
                            vm.username = username;
                            vm.admin = admin;
                            $.ajax({

                            });
                        }
                    }
                }
            });
        });
    }
}

$(document).ready(function () {
    userTable.Initialize();
    addButton.Initialize();
});