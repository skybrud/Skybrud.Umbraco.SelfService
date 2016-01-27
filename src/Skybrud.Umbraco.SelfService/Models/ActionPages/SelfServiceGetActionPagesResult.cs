namespace Skybrud.Umbraco.SelfService.Models.ActionPages {
    
    public class SelfServiceGetActionPagesResult {

        public SelfServiceGetActionPagesPaginationResult Pagination { get; private set; }

        public SelfServiceActionPageResult[] Items { get; private set; }

        public SelfServiceGetActionPagesResult(SelfServiceGetActionPagesPaginationResult pagination, SelfServiceActionPageResult[] items) {
            Pagination = pagination;
            Items = items;
        }

    }

}