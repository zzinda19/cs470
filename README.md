# Senior Software Capstone Project for John Carroll University: Anonymization Tool
## Project Overview
The intent of the anonymization program is to give users a tool to upload identifiable accession numbers and get anonymized accession numbers in return, to be used for third-party research. The program will allow users to upload a file (.csv, .xls, .xlsx) of accession numbers, which are then validated and added to that project's database. The users will also be able to download tables of the original accessions, anonymized accessions, and their associated MRNs (medical record numbers). Users who initially create a research project can also give/rescind access to other users for that project.
## Project Structure
1. /Content/Scripts/Dashboard: Relevant JS files. 
2. /Controllers: Standard MVC Controllers.
3. /Controllers/Api: Web Api 2 Controllers.
4. /Dtos: Data transfer objects.
5. /Models: MVC Models, including Entity edmx file.
6. /Views/Dashboard: Primary cshtml views.
7. /Views/Dashboard/Partials: Three partial subviews inserted into the ProjectDashboard view.
## Additional Notes
1. This project uses primarily Web Api 2 controllers for server-side functionality. However, downloading key pairs is done strictly within the standard MVC Dashboard Controller.
2. The primary ProjectDashboard view contains three partial subviews--one for each additional tab. Each tab also has its own associated JavaScript file in the Content/Scripts/Dashboard folder.
3. For uploading accessions from a file, the project currently assumes that the accessions will be housed in the first column of the spreadsheet.
