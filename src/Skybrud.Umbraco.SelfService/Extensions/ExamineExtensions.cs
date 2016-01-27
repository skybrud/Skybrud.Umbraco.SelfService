using Examine;
using Examine.Providers;
using Examine.SearchCriteria;

namespace Skybrud.Umbraco.SelfService.Extensions {

    public static class ExamineExtensions {

        public static ISearchResults Search(this BaseSearchProvider searcher, ISearchCriteria searchParams, out int total) {
            ISearchResults results = searcher.Search(searchParams);
            total = results.TotalItemCount;
            return results;
        } 

    }

}