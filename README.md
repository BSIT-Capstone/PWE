# PWE
Web Application providing access and downloads to reporting services.

<h3>Our Application</h3>

PWE Reporting makes it easy for our client to download reports from their reporting services. The goal was for reports to be available to download from their company server without having to use their legacy application that was more prone to user error and application timeouts.


How does PWE Reporting make life easier?

PWE Reporting dynamically loads available reports along with their parameters by accessing reporting services.

PWE Reporting presents reports for users to select based on report name and description and select parameters needed to download. Reports are downloaded in EXCEL format to allow users to edit and make changes.

PWE Reporting ensures a user has permissions to access reports through Windows Authentication and lends ability for reports to be available through any computer with domain privileges.


Purpose

PWE Reporting is an application solution for downloading reports from a reporting server. The application serves as part of a business process for employees to edit and make changes to content stored at their data warehouse.


Steps for Business Process

1. PWE Reporting application is used to authenticate users to access and download reports.

2. Reports are downloaded in EXCEL format for users to open and edit the reports.

3. Once reports have been edited they will be saved with a template to upload the EXCEL file for their company server.

4. The EXCEL file will be uploaded to the server through the EXCEL template.

5. Changes will be made to the server data warehouse when a System Admin commits the uploaded EXCEL files containing the report edits.
