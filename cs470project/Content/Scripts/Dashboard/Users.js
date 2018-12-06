var userDataTable;
var researchProjectUserDto = {};

function SetupUserDataTable(projectId) {
    userDataTable = $("#researchProjectUsers").DataTable({
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
                    if (data == true) {
                        return "Admin";
                    } else {
                        return "User";
                    }
                }
            },
            {
                data: "researchUser.userId",
                render: function (data) {
                    return "<button class='btn-link js-delete' data-user-id=" + data + ">Remove</button>";
                }
            }
        ]
    });
}

function SetupRemoveButtonOnClickHandler(projectId) {
    userDataTable.on("click", ".js-delete", function () {
        var removeButton = $(this);
        // Pull userId from the remove button's data-user-id attribute.
        var userId = removeButton.attr("data-user-id");

        bootbox.confirm("Are you sure you want to remove this user from the project?", function (result) {
            if (result) {
                $.ajax({
                    // Calls DeleteResearchProjectUser in the Api ResearchUsers Controller.
                    url: "/Api/ResearchUsers/" + projectId + "/" + userId,
                    method: "DELETE",
                    success: function () {
                        // If successful remove the user's row and redraw DataTable.
                        userDataTable.row(removeButton.parents("tr")).remove().draw();
                    },
                    error: function (xhr) {
                        toastr.error("An error occured: " + xhr.responseJSON.message);
                    }
                });
            }
        });
    });
}

function GetResearchUsers() {
    // Uses Bloodhound Package to Query ResearchUsers as the User Inputs a Username
    return new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('username'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Api/Users?query=%QUERY',
            wildcard: '%QUERY'
        }
    });
}

function SetupAddUserModalForm(form) {
    // Creates the form modal using the Bootbox Package.
    var addUserModal = bootbox.dialog({
        title: "Add User",
        // Must pass in form not form.html() otherwise the Typeahead package cannot find the input field by id.
        message: form,
        buttons: {
            cancel: {
                label: "Cancel",
                className: "btn btn-default",
                callback: function () {
                    // Clear username input field when modal is dismissed.
                    $("#username").typeahead("val", "");
                    researchProjectUserDto = {};
                }
            },
            ok: {
                label: "Submit",
                className: "btn btn-primary",
                callback: async function () {
                    // Clear username input field when modal is dismissed.
                    $("#username").typeahead("val", "");

                    // Pull admin value from form and add to Dto.
                    var admin = $("input[name='admin']:checked").val();
                    researchProjectUserDto.admin = admin;

                    var result;

                    // Transfer the ResearchProjectUserDto to the Api Controller using Ajax.
                    result = await $.ajax({
                        // Calls AddResearchUserToProject in the Api ResearchUsers Controller.
                        url: "/Api/ResearchUsers/Add",
                        method: "POST",
                        data: researchProjectUserDto,
                        success: function () {
                            userDataTable.ajax.reload();
                        },
                        error: function (xhr) {
                            toastr.error("An error occured: " + xhr.responseJSON.message);
                        }
                    });

                    researchProjectUserDto = {};
                    return result;
                }
            }
        }
    });

    // Must use .init to initialize Typeahead. Otherwise if the modal is reopened Typeahead doesn't work.
    addUserModal.init(function () {
        // Get the list of ResearchUsers using Bloodhound.
        var researchUsers = GetResearchUsers();
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
            researchProjectUserDto.userId = researchUser.userId;
        });
    });
}

function SetupAddUserButtonOnClickHandler(projectId) {
    // AddUserForm must be set upon page initialization and passed to the BootBox modal.
    // Otherwise, the modal has no way to access the form if the modal is reopened.
    var addUserForm = $("#addUserForm");
    var addUserButton = $("#addUserButton");
    addUserButton.click(function () {
        researchProjectUserDto.projectId = projectId;
        SetupAddUserModalForm(addUserForm);
    });
}

$(document).ready(function () {
    var projectId = $("#projectID").val();
    SetupUserDataTable(projectId);
    SetupRemoveButtonOnClickHandler(projectId);
    SetupAddUserButtonOnClickHandler(projectId);
});