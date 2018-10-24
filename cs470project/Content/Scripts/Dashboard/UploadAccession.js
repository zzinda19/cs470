$(document).ready(function () {
    var table = $("#researchAccessions").DataTable({
        ajax: { 
            url: "/Api/ResearchAccessions",
            dataSrc: ""
        },
        columns: [
            {
                data: "accession"
            }
        ]
    });
});