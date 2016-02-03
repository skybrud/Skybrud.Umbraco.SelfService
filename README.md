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
You have to run a installation-url (in your browser), for Skybrud.Umbraco.SelfService to be able to create documenttypes, datatypes and structure in your solution. 

  1. DocTypes installed
      - Selvbetjening - Handlingsside
      - Selvbetjening - Handlingssider
      - Selvbetjening - Kategori
      - Selvbetjening - Kategorier
      - Selvbetjeningsmodul
      
  2. Datatypes installed
      - # Selvbetjening - Punktopstilling
      - # Selvbetjening - Kategorivælger
      - # Selvbetjening - Handlingssider
      
  3. Structure installed (Content)
      - Moduler
        - Selvbetjening
          - Kategorier
          - Handlingssider
          

If you have an existing container for your modules, please add the nodeId from this container to your installation-url. Eg.: */umbraco/api/selfservice/install/?parentId=1090*

else just run */umbraco/api/selfservice/install/* in your favorite browser.

3. Change your default View for the "Handlingsside visning" doctype
  1. Edit "Selvbetjening - Handlingssider"
  2. Tab: Structure
  3. "Opret brugerdefineret liste" + Save
  4. Choose "Edit/Rediger"
  5. PropertyEditor: Skybrud Selvbetjening - Handlingssider

4. Create a Url-Provider to handle your virtual urls

```C#
namespace solution.Routing {
  public class UrlProvider : IUrlProvider {
    public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode) {
      try {
        IPublishedContent content = umbracoContext.ContentCache.GetById(id);
        
        if (content == null) return null;
        
        if(content.DocumentTypeAlias == "SkySelfServiceActionPage") {
          return string.Format("/selvbetjening/{0}/", content.UrlName);
        }
      } catch(Exception ex) {
        LogHelper.Error<UrlProvider>("Problem in Custom UrlProvider: ", ex);
      }
      
      return null;
    }
  }
}
```


5. Create a ContentFinder to handle the urls
```C#
namespace solution.Routing {
  public class ContentFinder : IContentFinder {
    public bool TryFindContent(PublishedContentRequest request) {
      // Get the URL, skip the query string and trim any trailing slashes (so we can assume that they're not there)
      string url = Request.RawUrl.Split('?')[0].TrimEnd('/');

      Match match = RegEx.Match(url, "^/selvbetjening/(.+?)$");
      
      if(!match.Success) continue;
      
      IPublishedContent content = GetSingleContentFromExamine(request.Uri.AbsolutePath.Split('/').Last().TrimEnd(), "SkySelfServiceActionPage");
      
      if (!request.HasTemplate) {
        request.TrySetTemplate("templateAlias");
      }

      request.PublishedContent = content;
    }
    
    
    private IPublishedContent GetSingleContentFromExamine(string nodename, string documentTypeAlias) {
            // Get a reference to the external searcher
            BaseSearchProvider externalSearcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];

            // Create a new search criteria and set our query
            ISearchCriteria criteria = externalSearcher.CreateSearchCriteria();
            criteria = criteria.RawQuery(string.Format("nodeTypeAlias_lci:{0}  +urlName_lci:{1}", documentTypeAlias.ToLower(),
                    nodename.ToLower()));

            // Get the first search result (since there really shouldn't be more with the same GUID)
            SearchResult first = externalSearcher.Search(criteria).FirstOrDefault();

            // Get the content node based on the Umbraco content id
            return first == null ? null : UmbracoContext.Current.ContentCache.GetById(first.Id);
        }
  }
}
``` 


6. Create a picker of your choice (NuPicker, MNTP, etc)

7. Create template for "Selvbetjening - Handlingsside" and add it to the DocType "SkySelfServiceActionPage"





[NuGetPackage]: https://www.nuget.org/packages/Skybrud.Umbraco.SelfService
[GitHubRelease]: https://github.com/skybrud/Skybrud.Umbraco.SelfService

