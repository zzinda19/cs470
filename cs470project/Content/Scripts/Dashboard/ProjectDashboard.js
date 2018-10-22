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
        uploadForm.SetUploadFileHandler();
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
        $("#uploadForm").on("submit", function () {
            toastr.success("submitted");
            console.log("Submitted");
            var fileInput = $("#fileInput");
            var file = fileInput[0].files[0];

            $.ajax({
                url: '/Api/File/Upload',
                method: 'post',
                data: file,
                success: function () {
                    toastr.success("It works");
                },
                error: function(xhr) {
                    toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                }
            });
        });
    }
};

$(document).ready(function () {
    layout.Initialize();
    uploadForm.Initialize();
});