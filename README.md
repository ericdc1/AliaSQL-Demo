AliaSQL-Demo
============
This is a simple single table crud app with some contrived business rules so there is something to unit test. It is a good kickstarter project if you use this stack. It uses the following:

* ASP.Net MVC 5
* Structuremap IOC container
* StackExchange Exceptional error logging handling
* StackExchange MiniProfiler 
* Dapper data access
* Dapper SimpleCRUD extensions to Dapper
* SimpleCRUD T4 template for POCO generation
* Automapper 
* Bootstrap generic UI
* Microsoft bunding and minification
* AliaSQL database deployment tool
* Assumes you have SQL Server 2008 or 2012 Express installed

Try out AliaSQL using the following scripts:

* build.bat - compile, update database, and run unit tests
* build_and_package.bat - compile, update database, run unit tests, and package into a zip file
* database-rebuild.bat - drops database and rebuilds it from scripts
* database-testdata.bat - runs any testdata scripts that haven't previously ran
* database-update.bat -  runs any create or update scripts that haven't previously ran
* database-diff.bat - runs sqlpackage.exe to compare the database scripts against the current sqlexpress database to generate a schema change script
* open_solution.bat - opens the .sln file in the source folder
* run_in_browser.bat - launches the web app in IIS Express
* default.ps1 - the psake build script
* schema-compare.ps1 - runs sqlpackage.exe to compare the database scripts against the current sqlexpress database to generate a schema change - script without a dependency on psake

To run from a build server you can pass parameters into the build_and_package.bat like this:

```dos
build_and_package.bat databaseServer = '.\sqlexpress'; version = '1.1.1' ; build_dir = 'c:\test' ; visualstudioversion = '12.0'
```
Here is a video demo: http://www.youtube.com/watch?v=oLC9MZGBFII
