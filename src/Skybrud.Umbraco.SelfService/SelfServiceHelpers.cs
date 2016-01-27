using System;
using System.Linq;
using Examine;
using Examine.Providers;
using Examine.SearchCriteria;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.SelfService {

    public static class SelfServiceHelpers {

        public static int GetContentIdFromGuid(string guid) {
            return GetContentIdFromGuid(Guid.Parse(guid));
        }

        public static int GetContentIdFromGuid(Guid guid) {

            // Get a reference to the searcher
            BaseSearchProvider searcher = ExamineManager.Instance.SearchProviderCollection["InternalSearcher"];

            // Create a new search criteria and set our query
            ISearchCriteria criteria = searcher.CreateSearchCriteria();
            criteria = criteria.RawQuery("key:" + guid);

            // Make the search in Examine
            ISearchResults results = searcher.Search(criteria);

            // Get the ID of the first result (or "0" if not found)
            return results.TotalItemCount == 0 ? 0 : results.First().Id;

        }

        public static IPublishedContent GetContentFromGuid(string guid) {
            return GetContentFromGuid(Guid.Parse(guid));
        }

        public static IPublishedContent GetContentFromGuid(Guid guid) {
            int id = GetContentIdFromGuid(guid);
            return id > 0 && UmbracoContext.Current != null ? UmbracoContext.Current.ContentCache.GetById(id) : null;
        }

    }

}