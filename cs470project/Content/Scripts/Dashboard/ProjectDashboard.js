/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    10.26.18
*  Description:     This populates the project dashboard with the appropriate project information
*/
var Layout = {
    RejectedAccessionsDataTable: {},
    UploadSpinner: {},

    Initialize: function () {
        this.RejectedAccessionsDataTable = $("#rejectedAccessions");
        this.UploadSpinner = $("#uploadSpinner");

        Layout.PopulateDashboard();
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


/**
*  Author:          Zak Zinda
*  Updated By:      Landry Snead
*  Date Updated:    12.4.18
*  Description:     This handles the client side end of uploading
*/
var UploadForm = {

    Initialize: function () {
        UploadForm.SetBrowseFileHandler();
        UploadForm.SetUploadFileHandler();
    },

    SetBrowseFileHandler: function () {
        var fileInput = $("#fileInput");
        $("#fileName").val("");

        //display selected file name
        fileInput.on("change", function () {
            var file = fileInput[0].files[0];
            var fileName = file.name;
            $("#fileName").val(fileName);
        });
    },


    SetUploadFileHandler: function () {
        var id = $("#projectID").val();

        $("#uploadForm").on("submit", function (e) {
            // Prevent the default form action so that the page is not refreshed upon user upload.
            e.preventDefault();

            //display upload spinner to indicate uploading is in progress
            Layout.UploadSpinner.css("font-size", "24px");

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

                    //remove upload spinner
                    Layout.UploadSpinner.css("font-size", "0px");

                    if (data.length > 0) {
                        //display rejected accession numbers
                        $("#uploadedMessage").text("Some of your accession numbers may have been rejected. Reference this table to see why.");
                        Layout.RejectedAccessionsDataTable.css("display", "block");

                        //clear data if a table already exists
                        if ($.fn.DataTable.isDataTable("#rejectedAccessions")) {
                            Layout.RejectedAccessionsDataTable.DataTable().clear().destroy();
                        }

                        Layout.RejectedAccessionsDataTable.DataTable({
                            data: data,
                            columns: [
                                { data: "accession" },
                                { data: "reason" }
                            ],
                            "autoWidth": true,
                            "fnCreatedRow": function (nRow) {
                                $(nRow).attr('class', 'danger');
                            }
                        });

                        Layout.RejectedAccessionsDataTable.columns.adjust().draw();

                        console.log(data);
                    }  
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


/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    10.26.18
*  Description:     On page load initializes the layout and upload form button.
*/
$(document).ready(function () {
    Layout.Initialize();
    UploadForm.Initialize();
});