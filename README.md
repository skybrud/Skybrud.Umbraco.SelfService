Skybrud.Umbraco.SelfService
========================

**Skybrud.Umbraco.SelfService** is a modulebased plugin for handling SelfService-pages in a municipality.

The package can handle categories and pages, so you only have one place to administer your Actionpages (handlingssider).

## Installation

1. [**NuGet Package**][NuGetPackage]
Install this NuGet package in your Visual Studio project. If you have both a web and a code project, install in both.

1. [**ZIP file**][GitHubRelease]
Grab a ZIP file of the latest release; unzip and move the contents to the root directory of your web application.

2. Run installation url to install data in Umbraco
If you have an existing container for your modules, please add the nodeId from this container to your installation-url. Eg.: /umbraco/api/selfservice/install/?parentId=1090

else just run /umbraco/api/selfservice/install/ in your favorite browser.

3. Change your default View for the "Handlingsside visning" doctype
3.1 Edit "Selvbetjening - Handlingssider"
3.2 Tab: Structure
3.3 "Opret brugerdefineret liste" + Save
3.4 Choose "Edit/Rediger"
3.5 PropertyEditor: Skybrud Selvbetjening - Handlingssider

4. Create a Url-Provider to handle your virtual urls

5. Create a ContentFinder to handle the urls

6. Create a picker of your choice (NuPicker, MNTP, etc)

7. Create template for "Selvbetjening - Handlingsside" and add it to the DocType "SkySelfServiceActionPage"





[NuGetPackage]: https://www.nuget.org/packages/Skybrud.Umbraco.SelfService
[GitHubRelease]: https://github.com/skybrud/Skybrud.Umbraco.SelfService

