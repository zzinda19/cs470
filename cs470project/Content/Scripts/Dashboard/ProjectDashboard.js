var layout = {
    Initialize: function () {
        layout.PopulateDashboard();
    },

    PopulateDashboard: function () {
        var id = $("#projectID").val();

        $.ajax({
            url: "/Api/ResearchProjects/" + id,
            success: function (data) {
                $("#header").text(data.projectName);
                $("#description").text(data.projectDescription);
                $("#insertDate").text(data.insertDate);
            },
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
            }
        });
    }
};

var uploadForm = {
    Initialize: function () {
        uploadForm.SetBrowseFileHandler();
    },

    SetBrowseFileHandler: function () {
        var fileInput = $("#fileInput");

        fileInput.on("change", function () {
            console.log(fileInput);
            var file = fileInput[0].files[0];
            var fileName = file.name;
            $("#fileName").val(fileName);
        });
    },

    SetUploadFileHandler: function () {
        // Create Submit Handler
        // Create Api Controller + Method, i.e. /Api/FileController/Upload
        // Use AJAX call and submit file using Data: file.
        // API Method can call the existing controller we've made and pass through file data.
        // API Method should return data in the form of IEnumerable or List.
        // Create datatable using returned data if the user presses the Preview Button.
    }
};

$(document).ready(function () {
    layout.Initialize();
    uploadForm.Initialize();
});