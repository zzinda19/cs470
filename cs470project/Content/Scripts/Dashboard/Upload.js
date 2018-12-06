/**
*  Author:          Zak Zinda
*  Updated By:      Landry Snead
*  Date Updated:    12.6.18
*  Description:     Initializes the upload form for accessions and handles user uploads as well as
 *                  creates the datatable of rejected accession numbers.
*/
var UploadForm = {

    Initialize: function () {
        UploadForm.SetBrowseFileHandler();
        UploadForm.SetUploadFileHandler();
    },

    SetBrowseFileHandler: function () {
        var fileInput = $("#fileInput");
        $("#fileName").val("");

        // Displays the selected file name to the user in the file input box.
        fileInput.on("change", function () {
            var file = fileInput[0].files[0];
            var fileName = file.name;
            $("#fileName").val(fileName);
        });
    },

    SetUploadFileHandler: function () {
        var id = $("#projectID").val();

        $("#uploadForm").on("submit", function (e) {
            // Prevent the page from refreshing upon user upload.
            e.preventDefault();

            // Display upload spinner to indicate uploading is in progress.
            var uploadSpinner = $("#uploadSpinner");
            uploadSpinner.css("font-size", "24px");

            // Obtain the submitted file.
            var fileInput = $("#fileInput");
            var file = fileInput[0].files[0];

            // Discern whether or not the file has a header.
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

                    // Hide upload spinner.
                    uploadSpinner.css("font-size", "0px");

                    if (data.length > 0) {
                        // Display rejected accession numbers.
                        $("#uploadedMessage").text("Some of your accession numbers were rejected. Reference this table to see why.");
                        UploadForm.SetupRejectedDataTable(data);
                    }
                },
                error: function (xhr) {
                    toastr.error("An error occured: " + xhr.responseJSON.message);
                }
            });

            // Return false so the page does not refresh.
            return false;
        });
    },

    // Setup the rejected accessions data table.
    SetupRejectedDataTable: function (data) {
        var table = $("#rejectedAccessions");

        // Clear data if the table already exists.
        if ($.fn.DataTable.isDataTable(table)) {
            table.DataTable().clear().destroy();
        }

        // Initialize new table.
        table.DataTable({
            data: data,
            columns: [
                { data: "accession" },
                { data: "reason" }
            ],
            "fnCreatedRow": function (nRow) {
                // Set rejected rows to class 'danger'.
                $(nRow).attr('class', 'danger');
            }
        });

        // Show table.
        table.show();
    }
};

$(document).ready(function () {
    UploadForm.Initialize();
});