using System;

namespace Skybrud.Umbraco.SelfService.Models.ActionPages {
    
    public class SelfServiceGetActionPagesPaginationResult {

        public int Page { get; private set; }

        public int Pages { get; private set; }

        public int Limit { get; private set; }

        public int Offset { get; private set; }

        public int Total { get; private set; }

        public int From { get; private set; }

        public int To { get; private set; }

        public SelfServiceGetActionPagesPaginationResult(int page, int pages, int limit, int offset, int total, int count) {
            Page = page;
            Pages = pages;
            Limit = limit;
            Offset = offset;
            Total = total;
            From = offset + 1;
            To = Math.Min(offset + limit, count);
        }

    }

}