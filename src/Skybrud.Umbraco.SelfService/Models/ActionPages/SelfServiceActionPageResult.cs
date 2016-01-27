using Examine;

namespace Skybrud.Umbraco.SelfService.Models.ActionPages {
    
    public class SelfServiceActionPageResult {

        public double Relevance {
            get { return Result.Score; }
        }

        public SearchResult Result { get; private set; }

        public SelfServiceActionPage Page { get; private set; }

        public SelfServiceActionPageResult(SearchResult result, SelfServiceActionPage page) {
            Result = result;
            Page = page;
        }

    }

}