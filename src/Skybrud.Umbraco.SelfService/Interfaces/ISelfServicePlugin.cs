using Examine;
using Skybrud.Umbraco.SelfService.Models.ActionPages;
using Skybrud.Umbraco.SelfService.Models.Categories;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.SelfService.Interfaces {
    
    /// <summary>
    /// Interface describing a plugin for the self service module. If you do not wish to implement the entire
    /// interface, have a look at the <code>SelfServicePluginBase</code> class instead.
    /// </summary>
    public interface ISelfServicePlugin {

        /// <summary>
        /// Parses the specified <code>result</code> into an instance of <code>SelfServiceActionPage</code>.
        /// </summary>
        /// <param name="result">An instance of <code>SearchResult</code> representing the action page.</param>
        /// <returns>Returns an instance of <code>SelfServiceActionPage</code>.</returns>
        SelfServiceActionPage ParseActionPage(SearchResult result);

        /// <summary>
        /// Parses the specified <code>content</code> into an instance of <code>SelfServiceCategory</code>.
        /// </summary>
        /// <param name="content">An instance of <code>IPublishedContent</code> representing the category.</param>
        /// <param name="parent">The parent category.</param>
        /// <returns>Returns an instance of <code>SelfServiceCategory</code>.</returns>
        SelfServiceCategory ParseCategory(IPublishedContent content, SelfServiceCategory parent);

    }

}