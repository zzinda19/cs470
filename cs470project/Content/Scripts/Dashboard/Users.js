/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    12.6.18
*  Description:     Initializes the users datatable, handles adding and deleting users
*                   from a project.
*/
var Users = {

    UserDataTable: {},
    ResearchProjectUserDto: {},

    Initialize: function () {
        var projectId = $("#projectID").val();
        Users.SetupUserDataTable(projectId);
        Users.SetupRemoveButtonOnClickHandler(projectId);
        Users.SetupAddUserButtonOnClickHandler(projectId);
    },

    // Initializes the users datatable.
    SetupUserDataTable: function (projectId) {
        this.UserDataTable = $("#researchProjectUsers").DataTable({
            ajax: {
                // Calls GetResearchProjectUsers in the Api ResearchUsers Controller.
                url: "/Api/ResearchUsers/" + projectId,
                dataSrc: "",
                error: function (xhr) {
                    toastr.error("An error occured: " + xhr.responseJSON.message);
                }
            },
            columns: [
                {
                    data: "researchUser.username"
                },
                {
                    data: "admin",
                    render: function (data) {
                        // Admin is of boolean type.
                        // Return "Admin" if true, "User" if false.
                        if (data == true) {
                            return "Admin";
                        } else {
                            return "User";
                        }
                    }
                },
                {
                    data: "researchUser.userId",
                    // Renders a link to remove a user from the project based upon their user id.
                    // data-user-id is a custom attribute used in the Remove Button on-click handler.
                    render: function (data) {
                        return "<button class='btn-link js-delete' data-user-id=" + data + ">Remove</button>";
                    }
                }
            ]
        });
    },

    SetupRemoveButtonOnClickHandler: function (projectId) {
        this.UserDataTable.on("click", ".js-delete", function () {
            var removeButton = $(this);
            // Pull the user's id from the remove button's data-user-id custom attribute.
            var userId = removeButton.attr("data-user-id");

            bootbox.confirm("Are you sure you want to remove this user from the project?", function (result) {
                if (result) {
                    $.ajax({
                        // Calls DeleteResearchProjectUser in the Api ResearchUsers Controller.
                        url: "/Api/ResearchUsers/" + projectId + "/" + userId,
                        method: "DELETE",
                        success: function () {
                            // If successful remove the user's row and redraw DataTable.
                            Users.UserDataTable.row(removeButton.parents("tr")).remove().draw();
                        },
                        error: function (xhr) {
                            toastr.error("An error occured: " + xhr.responseJSON.message);
                        }
                    });
                }
            });
        });
    },

    SetupAddUserButtonOnClickHandler: function (projectId) {
        // AddUserForm must be set upon page initialization and passed to the BootBox modal.
        // Otherwise, the modal has no way to access the form if the modal is reopened.
        var addUserForm = $("#addUserForm");
        var addUserButton = $("#addUserButton");
        addUserButton.click(function () {
            Users.ResearchProjectUserDto.projectId = projectId;
            Users.SetupAddUserModalForm(addUserForm);
        });
    },

    SetupAddUserModalForm: function(form) {
        // Creates the form modal using the Bootbox Package.
        var addUserModal = bootbox.dialog({
            title: "Add User",
            // Must pass in form not form.html() otherwise the Typeahead package cannot find the input field by id.
            message: form,
            closeButton: false,
            buttons: {
                cancel: {
                    label: "Cancel",
                    className: "btn btn-default",
                    callback: function () {
                        Users.ResetUserForm();
                    }
                },
                ok: {
                    label: "Submit",
                    className: "btn btn-primary",
                    callback: function () {
                        Users.SubmitUserForm();
                        Users.ResetUserForm();
                    }
                }
            }
        });

        // Must use .init to initialize Typeahead. Otherwise if the modal is reopened Typeahead doesn't work.
        addUserModal.init(function () {
            // Get the list of ResearchUsers using Bloodhound.
            var researchUsers = BloodhoundModule.GetResearchUsers();
            // Set the Typeahead parameters for the username field.
            $("#username").typeahead({
                hint: true,
                highlight: true,
                minLength: 3
            },
            {
                name: 'researchUsers',
                display: 'username',
                source: researchUsers
            }).on("typeahead:selected", function (e, researchUser) {
                // Once a user is selected, add their id to the Dto.
                Users.ResearchProjectUserDto.userId = researchUser.userId;
            });
        });
    },

    // Submits user form data using Ajax.
    SubmitUserForm: function () {
        // Pull admin value from form and add to Dto.
        var admin = $("input[name='admin']:checked").val();
        Users.ResearchProjectUserDto.admin = admin;

        // Transfer the ResearchProjectUserDto to the Api Controller using Ajax.
        $.ajax({
            // Calls AddResearchUserToProject in the Api ResearchUsers Controller.
            url: "/Api/ResearchUsers/Add",
            method: "POST",
            data: Users.ResearchProjectUserDto,
            success: function () {
                Users.UserDataTable.ajax.reload();
            },
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.responseJSON.message);
            }
        });
    },

    // Clears username field and resets ResearchProjectDto.
    ResetUserForm: function () {
        $("#username").typeahead("val", "");
        Users.ResearchProjectUserDto = {};
    }
}

// Bloodhound Module, queries users based upon the username entered in the add user form.
var BloodhoundModule = {
    GetResearchUsers: function () {
        // Uses Bloodhound plug-in to query ResearchUsers as the user inputs a username.
        return new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('username'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/Api/Users?query=%QUERY',
                wildcard: '%QUERY'
            }
        });
    }
}

$(document).ready(function () {
    Users.Initialize();
});