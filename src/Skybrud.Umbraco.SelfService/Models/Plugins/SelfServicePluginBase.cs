using Examine;
using Skybrud.Umbraco.SelfService.Interfaces;
using Skybrud.Umbraco.SelfService.Models.ActionPages;
using Skybrud.Umbraco.SelfService.Models.Categories;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.SelfService.Models.Plugins {

    /// <summary>
    /// Abstract class implementing the <code>ISelfServicePlugin</code> interface. You can inherit from this class
    /// rather than the interface if you do not wish to implement the entire interface.
    /// </summary>
    public abstract class SelfServicePluginBase : ISelfServicePlugin {

        /// <summary>
        /// Parses the specified <code>result</code> into an instance of <code>SelfServiceActionPage</code>.
        /// </summary>
        /// <param name="result">An instance of <code>SearchResult</code> representing the action page.</param>
        /// <returns>Returns an instance of <code>SelfServiceActionPage</code>.</returns>
        public SelfServiceActionPage ParseActionPage(SearchResult result) {
            return null;
        }

        /// <summary>
        /// Parses the specified <code>content</code> into an instance of <code>SelfServiceCategory</code>.
        /// </summary>
        /// <param name="content">An instance of <code>IPublishedContent</code> representing the category.</param>
        /// <param name="parent">The parent category.</param>
        /// <returns>Returns an instance of <code>SelfServiceCategory</code>.</returns>
        public virtual SelfServiceCategory ParseCategory(IPublishedContent content, SelfServiceCategory parent) {
            return null;
        }

    }

}