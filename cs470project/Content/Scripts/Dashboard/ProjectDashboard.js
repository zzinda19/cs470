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
            console.log(fileName);
            $("#fileName").val(fileName);
        });
    },

    SetUploadFileHandler: function () {
        $("#uploadForm").on("submit", function (e) {
            e.preventDefault();

            var fileInput = $("#fileInput");

            console.log(fileInput);
            var file = fileInput[0].files[0];

            console.log(file);

            var formData = new FormData();
            formData.append("uploadFile", file);
            console.log(formData);

            $.ajax({
                url: "/Api/File/Upload",
                method: "post",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    toastr.success(data);
                },
                error: function (xhr) {
                    toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                }
            });
            
            return false;
        });
    }
};

$(document).ready(function () {
    layout.Initialize();
    uploadForm.Initialize();
});