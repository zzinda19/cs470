/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    10.26.18
*  Description:     This populates the project dashboard with the appropriate project information
*/
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


/**
*  Author:          Zak Zinda
*  Updated By:      Landry Snead
*  Date Updated:    12.4.18
*  Description:     This handles the client side end of uploading
*/
var uploadForm = {

    Initialize: function () {
        uploadForm.SetBrowseFileHandler();
        uploadForm.SetUploadFileHandler();
    },

    SetBrowseFileHandler: function () {
        var fileInput = $("#fileInput");
        $("#fileName").val("");

        //display selected file name
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

            //display upload spinner to indicate uploading is in progress
            document.getElementById("uploadSpinner").style.fontSize = "24px";

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
                    document.getElementById("uploadSpinner").style.fontSize = "0px";

                    if (data.length > 0) {
                        //display rejected accession numbers
                        document.getElementById("uploadedMessage").innerHTML = "Some of your accession numbers may have been rejected. Reference this table to see why";
                        document.getElementById("researchAccessions").style.display = "block";

                        //clear data if a table already exists
                        if ($.fn.DataTable.isDataTable("#researchAccessions")) {
                            $('#researchAccessions').DataTable().clear().destroy();
                        }

                        $("#researchAccessions").DataTable({
                            data: data,
                            columns: [
                                { data: "accession" },
                                { data: "reason" }
                            ],
                            "fnCreatedRow": function (nRow) {
                                $(nRow).attr('class', 'danger');
                            }
                        });
                    }
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


/**
*  Author:          Zak Zinda
*  Updated By:      
*  Date Updated:    10.26.18
*  Description:     On page load hide elements and intialize the layout and uploadForm functions
*/
$(document).ready(function () {
    //when the page is loaded, do not display the rejected accessions table or upload spinner
    document.getElementById("researchAccessions").style.display = "none";
    document.getElementById("uploadSpinner").style.fontSize = "0px";
    layout.Initialize();
    uploadForm.Initialize();
});