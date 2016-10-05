using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Providers;
using Examine.SearchCriteria;
using Skybrud.Umbraco.SelfService.Enums;
using Skybrud.Umbraco.SelfService.Extensions;
using Skybrud.Umbraco.SelfService.Models.Plugins;

namespace Skybrud.Umbraco.SelfService.Models.ActionPages {

    /// <summary>
    /// Repository class for working with action pages.
    /// </summary>
    public class SelfServiceActionPagesRepository {

        #region Properties

        /// <summary>
        /// Gets a reference to the parent self service context.
        /// </summary>
        public SelfServiceContext Context { get; private set; }

        #endregion

        #region Constructors

        internal SelfServiceActionPagesRepository(SelfServiceContext context) {
            Context = context;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Gets an array of all action pages matching <code>rawQuery</code> in the specified <code>index</code>.
        /// </summary>
        /// <param name="rawQuery">The raw Examine query.</param>
        /// <param name="index">The index to be used for the search.</param>
        /// <returns>Returns an array of <code>SelfServiceActionPage</code>.</returns>
        public SelfServiceActionPage[] GetActionPages(string rawQuery, SelfServiceIndexType index) {

            // Determine the searcher name
            string searcherName = index == SelfServiceIndexType.Internal ? "InternalSearcher" : "ExternalSearcher";

            // Get a reference to the searcher
            BaseSearchProvider searcher = ExamineManager.Instance.SearchProviderCollection[searcherName];

            // Create a new search criteria and set our query
            ISearchCriteria criteria = searcher.CreateSearchCriteria();
            criteria = criteria.RawQuery(rawQuery);

            // Make the search in Examine
            ISearchResults results = searcher.Search(criteria);

            // Order the action pages by their name and return the list as an array
            return results.OrderBy(x => x.Fields["nodeName"]).Select(ParseActionPage).ToArray();

        }

        public SelfServiceGetActionPagesResult GetActionPages(SelfServiceGetActionPagesOptions options) {

            // Determine the searcher name
            string searcherName = options.IndexType == SelfServiceIndexType.Internal ? "InternalSearcher" : "ExternalSearcher";

            // Get a reference to the searcher
            BaseSearchProvider searcher = ExamineManager.Instance.SearchProviderCollection[searcherName];

            // Determine the current page
            int page = options.Limit == 0 ? 1 : (int) Math.Floor(options.Offset / (double) options.Limit) + 1;
            
            // Create a new search criteria and set our query
            ISearchCriteria criteria = searcher.CreateSearchCriteria();
            criteria = criteria.RawQuery(options.GetQuery());

            // Make the search in Examine
            int total;
            IEnumerable<SearchResult> results = searcher.Search(criteria, out total);

            // Calculate variables used for the pagination
            int limit = options.Limit;
            int pages = (int) Math.Ceiling(total / (double)limit);
            page = Math.Max(1, Math.Min(page, pages));
            int offset = (page * limit) - limit;

            // Sort the results based on the specified options
            switch (options.SortField) {
                case SelfServiceActionPageSortField.Id:
                    results = results.OrderBy(x => x.Id, options.SortOrder);
                    break;
                case SelfServiceActionPageSortField.Name:
                    results = results.OrderBy(x => x.Fields["nodeName"], options.SortOrder);
                    break;
                case SelfServiceActionPageSortField.CreatedAt:
                    results = results.OrderBy(x => x.Fields["createDate"], options.SortOrder);
                    break;
                case SelfServiceActionPageSortField.UpdatedAt:
                    results = results.OrderBy(x => x.Fields["updateDate"], options.SortOrder);
                    break;
                case SelfServiceActionPageSortField.CreatedBy:
                    results = results.OrderBy(x => x.Fields["creatorName"], options.SortOrder);
                    break;
                case SelfServiceActionPageSortField.UpdatedBy:
                    results = results.OrderBy(x => x.Fields["writerName"], options.SortOrder);
                    break;
                default:
                    throw new Exception("Unknown sort field: " + options.SortField);
            }

            // Apply pagination
            results = results.Skip(offset).Take(limit);

            // Generate the result
            return new SelfServiceGetActionPagesResult(
                new SelfServiceGetActionPagesPaginationResult(page, pages, limit, offset, total, results.Count()),
                (
                    from result in results
                    let actionPage = ParseActionPage(result)
                    select new SelfServiceActionPageResult(result, actionPage)
                ).ToArray()
            );

        }

        public SelfServiceActionPage ParseActionPage(SearchResult result) {

            // If "result" is NULL, we don't bother doing any further parsing
            if (result == null) return null;

            // Iterate through the added plugins
            foreach (SelfServicePluginBase plugin in Context.Plugins) {

                // Let the plugin parse the action page (NULL means the plugin didn't handle or parse the category)
                SelfServiceActionPage actionPage = plugin.ParseActionPage(result);
                if (actionPage != null) return actionPage;

            }
            
            // Fallback if no plugins did handle or parse the action page
            return SelfServiceActionPage.GetFromResult(result);

        }

        #endregion

    }

}