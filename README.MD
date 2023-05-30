The ContactManager allows to upload some raw format (CSV) preprepared contacts list from  file, 
automatically convert into the list of employee objects, and store  them in database and then display into the browser.
Looking through the employee contacts info (name, dob, married status, phone  and salary), one can filter or sort by any column. To filter married emplloyees type "true" in the field, or otherwise - something else.
User can also edit and delete contacts info inline. Validation for empty updating values is provided.
This project uses ASP.NET Core MVC, Entity Framework Core, MS SQL Server, and  jQuery as well as Bootstrap libraries.
The client side keeps the SPA principle, by using Ajax and partial views. So using jquery.unobtrusive-ajax.js as script is needed in main view.
Use commands- dotnet restore and dotnet run in cmdline, to lauch project.