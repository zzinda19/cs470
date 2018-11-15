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
        $("#fileName").val("");

        fileInput.on("change", function () {
            console.log(fileInput);
            var file = fileInput[0].files[0];
            var fileName = file.name;
            console.log(fileName);
            $("#fileName").val(fileName);
        });
    },

    SetUploadFileHandler: function () {
        var id = $("#projectID").val();
        
        $("#uploadForm").on("submit", function (e) {
            // Prevent the default form action so that the page is not refreshed upon user upload.
            e.preventDefault();

            // Obtain the inputted file.
            var fileInput = $("#fileInput");
            var file = fileInput[0].files[0];

            // Obtain whether or not the file has a header.
            var headerValue = $("input[name='hasHeader']:checked").val();

            // Create FormData object and add the file and header values.
            var formData = new FormData();
            formData.append("uploadFile", file);
            formData.append("hasHeader", headerValue);

            // Post the form to the Api FileController Upload method.
            $.ajax({
                url: "/Api/File/" + id,
                method: "post",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    toastr.success("Accession numbers successfully uploaded.");
                    console.log(data);
                },
                error: function (xhr) {
                    toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                }
            });

            // Prevent page refresh upon submit.
            return false;
        });
    }
};

var downloadForm = {

    Initialize: function () {
        downloadForm.SetDownloadFileHandler();
    },

    SetDownloadFileHandler: function () {
        var id = $("#projectID").val();

        $("#downloadForm").on("submit", function (e) {
            e.preventDefault();

            var downloadType = $("input[name='downloadType']:checked").val();

            console.log(downloadType);

            var vm = {
                ProjectId: id,
                DownloadType: downloadType
            };

            $.ajax({
                url: "/Api/File/Download",
                method: "get",
                data: vm,
                success: function (data) {
                    window.location = "/Api/File/Download";
                },
                error: function (xhr) {
                    toastr.error("An error occured: " + xhr.status + " " + xhr.statusText);
                }
            });

            return false;
        });
    }
}

$(document).ready(function () {
    layout.Initialize();
    uploadForm.Initialize();
    downloadForm.Initialize();
});