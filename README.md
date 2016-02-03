Skybrud.Umbraco.SelfService
========================

**Skybrud.Umbraco.SelfService** is a module based plugin for handling self service pages for a municipality or similar.

In it's current state, the package will add logic for categories and action pages (handlingssider in Danish) that editors can manage via the Umbraco backoffice.

## List of contents

* [Installation](#installation)
* [Setup](#setup)
    * [Run the installer](#run-the-installer)
* [Using the module](#using-the-module)
    * [URL provider](#url-provider)
    * [Content finder](#content-finder)
    * [Startup](#startup)
* [Installed doctypes, datatypes and content](#installed-doctypes-datatypes-and-content)

## Installation

1. [**NuGet package**][NuGetPackage]  
Use NuGet to install the package in your Visual Studio project. If you have both a web and a code project, install the package in both projects.

1. [**ZIP file**][GitHubRelease]
Coming soon ;)

## Setup

#### Run the installer
Once you have installed the NuGet package, you will need to run the installer to set up the package. [The installer will add a number or content types and data types as well as add a few items in the content section](#installed-doctypes-datatypes-and-content).

The installer is currently just a WebApi method that you can call in your browser. When running the installer, it will by default create a new container called `Selvbetjening` (you can just rename this after the install) at the root level. This can be done by calling the following URL in your Umbraco installation:

```
/umbraco/api/selfservice/install/
```

If you wish the `Selvbetjening` node to be created under a specific node, you can specify the ID of that node as a parameter to the installer like the URL below:

```
/umbraco/api/selfservice/install/?parentId=1090
```

After running the installer, your content section should look something like this:

![image](https://cloud.githubusercontent.com/assets/3634580/12757389/23cf73ba-c9d8-11e5-95fd-7836b3ee98a0.png)

**Set up a list view for action pages (optional)**
All action pages should be created under the `Handlingssider` node (document type: `Selvbetjening - Handlingssider`). Clicking this node in the content section will display a list of action pages using the default Umbraco list view like shown in the screenshot below:

![image](https://cloud.githubusercontent.com/assets/3634580/12757452/5fe9e6dc-c9d8-11e5-8bfe-7d370e6b643f.png)

If this is fine, you can simply skip this step. Alternatively, you can update the document type to use a custom list view specific to action pages.

To do this, you can go to the Settings section, find the `Selvbetjening - Handlingssider` document type, and select the `Structure` tab. Here you can then click the `Create custom list view`, and then click the `Edit` link just above.

Clicking the `Edit` link will take you to a new data type Umbraco just created for you. Here you can select the `Skybrud Selvbetjening - Handlingssider` property editor rather than Umbraco's default `List view`. This will make the list from before look like the screenshot below (sorry for the Danish):

![image](https://cloud.githubusercontent.com/assets/3634580/12757721/7b6170fa-c9d9-11e5-8c4a-da6f3bb7785a.png)

With the custom list view, you will be able to filter the list by a free text search as well as by one or more of the categories created under the `Kategorier` node.

## Using the module

There are plenty of ways the self service module can be used. At Skybrud.dk, we have a very specific way of using it:

#### URL Provider
In our setup, the self service nodes lives outside of the normal site tree, so we need to add a URL provider so the editors will see some URLs that actually work.

The code for the URL provider looks something like this:

```C#
using System;
using System.Collections.Generic;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace SelfServiceTest.Routing {

    public class SelfServiceUrlProvider : IUrlProvider {

        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode) {
            
            try {
                
                // Attempt to get the content item from the cache
                IPublishedContent content = umbracoContext.ContentCache.GetById(id);

                // If the content iten matches our document type, we return our custom URL
                if (content != null && content.DocumentTypeAlias == "SkySelfServiceActionPage") {
                    return String.Format("/selvbetjening/{0}/", content.UrlName);
                }
            
            } catch (Exception ex) {

                // Append the exception to the Umbraco log
                LogHelper.Error<SelfServiceUrlProvider>("Unable to provide URL for action page: " + ex.Message, ex);
            
            }

            // NULL means the URL provider didn't provide an URL
            return null;
        
        }

        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current) {

            // The self service pages will in our case not have any secondary
            // URLs, so we just return an empty list at this point
            return new List<string>();

        }

    }

}
```

#### Content Finder
While the URL provider gives you a way to tell Umbraco the URL of each action page, this is just the visual part. For the actual requests to the action pages to work, we also need a content finder.

```C#
using System;
using System.Text.RegularExpressions;
using System.Web;
using Skybrud.Umbraco.SelfService;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace SelfServiceTest.Routing {

    public class SelfServiceContentFinder : IContentFinder {

        public bool TryFindContent(PublishedContentRequest request) {
            
            // Get the URL, skip the query string and trim any trailing slashes (so we can assume that they're not there)
            string url = HttpContext.Current.Request.RawUrl.Split('?')[0].TrimEnd('/');

            // Try to match the URL
            Match match = Regex.Match(url, "^/selvbetjening/(.+?)$");

            // If the URL doesn't match, we simply return FALSE (means we haven't found anything)
            if (!match.Success) return false;

            // Get the parent node of the action pages
            IPublishedContent parent = SelfServiceHelpers.GetContentFromGuid("942daec4-34cc-4114-9f9e-37c84c6de572");

            // Construct the XPath for finding the action page
            string xpath = "//SkySelfServiceActionPages[@id=" + parent.Id + "]/SkySelfServiceActionPage[@urlName='" + match.Groups[1].Value + "']";

            // Make the lookup in the content cache
            IPublishedContent content = UmbracoContext.Current.ContentCache.GetSingleByXPath(xpath);

            // If the action page doesn't already have a template, we specify one explicitly
            if (!request.HasTemplate) {

                // You should edit this to match your custom template
                request.TrySetTemplate("ActionPage");
            
            }

            // Set the content for the request
            request.PublishedContent = content;

            // Return whether we found anything
            return request.PublishedContent != null;

        }
    
    }

}
``` 

#### Startup
For both the URL provider and the content finder to work, you need to add them to Umbraco's logic during startup:

```C#
using SelfServiceTest.Routing;
using Umbraco.Core;
using Umbraco.Web.Routing;

namespace SelfServiceTest {
    
    public class Startup : ApplicationEventHandler {
        
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            
            // Register our custom content finder
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, SelfServiceContentFinder>();

            // Register our custom URL provider
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, SelfServiceUrlProvider>();
        
        }
    
    }

}
```

## Installed doctypes, datatypes and content

The following data will be installed in your solution

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



[NuGetPackage]: https://www.nuget.org/packages/Skybrud.Umbraco.SelfService
[GitHubRelease]: https://github.com/skybrud/Skybrud.Umbraco.SelfService/releases/latest
