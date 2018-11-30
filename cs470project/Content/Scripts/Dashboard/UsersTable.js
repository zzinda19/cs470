var projectId = $("#projectID").val();

var userTable = {
    Initialize: function () {
        // Create User DataTable Using Ajax
        var table = $("#researchProjectUsers").DataTable({
            ajax: {
                // Call GetResearchProjectUsers from the Api Users Controller
                url: "/Api/ResearchUsers/" + projectId,
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
                    data: "researchUser.userId",
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
                        url: "/Api/ResearchUsers/" + projectId + "/" + userId,
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

        var vm = {
            projectId: projectId,
        };

        var researchUsers = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('username'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/Api/Users?query=%QUERY',
                wildcard: '%QUERY'
            }
        });

        $('#username').typeahead({
            minLength: 3,
            highlight: true
        }, {
                name: 'researchUsers',
                display: 'username',
                source: researchUsers
            }).on("typeahead:select", function (e, researchUser) {
                vm.userId = researchUser.userId;
            });

        var form = $("#addUserForm");
        form.on("submit", function (e) {

            e.preventDefault();
            $("#username").typeahead("val", "");

            var admin = $("input[name='admin']:checked").val();
            vm.admin = admin;

            console.log(vm.projectId);
            console.log(vm.userId);
            console.log(vm.admin);

            $.ajax({
                url: "/Api/ResearchUsers/Add",
                method: "POST",
                data: vm,
                success: function () {
                    table.ajax.reload();
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                }
            });
        });
    }
}



var userForm = {
    Initialize: function () {
        
    }
}

$(document).ready(function () {
    userTable.Initialize();
    userForm.Initialize();

/*    button.on("click", function () {
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
                        var admin = $("input[name='admin']:checked").val();
                        vm.admin = admin;
                        if (!username) {
                            // If program user does not enter a username return an error.
                            toastr.error("Please input a username.");
                            return false;
                        }
                        $("#username").typeahead("val", "");
                        console.log(vm.projectId);
                        console.log(vm.userId);
                        console.log(vm.admin);
                    }
                }
            }
        });
    });*/
});