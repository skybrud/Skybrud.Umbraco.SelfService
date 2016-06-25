using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Skybrud.Umbraco.SelfService.Enums;

namespace Skybrud.Umbraco.SelfService.Models.ActionPages {

    public class SelfServiceGetActionPagesOptions {

        #region Properties

        /// <summary>
        /// Gets or sets whether the search should be based on the internal or external Examine index (default is <code>External</code>).
        /// </summary>
        public SelfServiceIndexType IndexType { get; set; }

        /// <summary>
        /// Gets or sets the text string the results should match.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets an array of categories the results should match.
        /// </summary>
        public int[] CategoryIds { get; set; }

        /// <summary>
        /// Gets or sets the field the results should be sorted by (default is <code>Name</code>).
        /// </summary>
        public SelfServiceActionPageSortField SortField { get; set; }

        /// <summary>
        /// Gets or sets the sort order of the results (default is <code>Ascending</code>).
        /// </summary>
        public SelfServiceSortOrder SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of results to return on each page (default is <code>30</code>).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the offset from where the pagination should start (default is <code>0</code>).
        /// </summary>
        public int Offset { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public SelfServiceGetActionPagesOptions() : this(30, 0) { }

        /// <summary>
        /// Initializes a new instance with default options and the specified <code>limit</code>.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public SelfServiceGetActionPagesOptions(int limit) : this(limit, 0) { }

        /// <summary>
        /// Initializes a new instance with default options and the specified <code>limit</code> and <code>offset</code>.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <param name="offset">The offset.</param>
        public SelfServiceGetActionPagesOptions(int limit, int offset) {
            IndexType = SelfServiceIndexType.External;
            CategoryIds = new int[0];
            SortField = SelfServiceActionPageSortField.Name;
            SortOrder = SelfServiceSortOrder.Ascending;
            Limit = limit;
            Offset = offset;
        }

        #endregion

        #region Member methods

        public List<object> GetQueryList() {
            List<object> query = new List<object>();
            AppendNodeTypeAlias(query);
            AppendText(query);
            AppendCategories(query);
            return query;
        }

        public string GetQuery() {
            return GetQuery(GetQueryList());
        }

        protected virtual string GetQuery(List<object> list) {

            if (list == null) return "";

            StringBuilder sb = new StringBuilder();

            foreach (object item in list) {
                if (item is string) {
                    sb.AppendLine(item + "");
                } else if (item is List<object>) {
                    sb.AppendLine("(" + GetQuery((List<object>) item) + ")");
                }
            }

            return sb.ToString();

        }

        protected virtual void AppendNodeTypeAlias(List<object> query) {
            if (query.Count > 0) query.Add("AND");
            query.Add("nodeTypeAlias:SkySelfServiceActionPage");
        }

        protected virtual void AppendText(List<object> query) {

            if (String.IsNullOrWhiteSpace(Text)) return;
            
            if (query.Count > 0) query.Add("AND");

            // Fields to search for the keywords
            List<string> fields = new List<string> { "nodeName", "title" };

            string lciQuery = PrepareSearchString(Text);

            query.Add(String.Join(" AND ", (
                from term in lciQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                let escaped = (
                    from field in fields
                    select String.Format("{1}:({0} {0}*)", term, Lucene.Net.QueryParsers.QueryParser.Escape(field))
                )
                select "(" + String.Join(" OR ", escaped) + ")"
            )));

        }

        protected virtual void AppendCategories(List<object> query) {

            if (CategoryIds == null || CategoryIds.Length == 0) return;

            if (query.Count > 0) query.Add("AND");

            List<object> categories = new List<object>();

            for (int i = 0; i < CategoryIds.Length; i++) {
                if (i > 0) categories.Add("OR");
                categories.Add("skySelfServiceCategories:" + CategoryIds[i]);
            }

            query.Add(categories);

        }

        private string PrepareSearchString(string query) {
            return Regex.Replace(query ?? "", @"[^\wæøåÆØÅ\- ]", "").ToLowerInvariant().Trim();
        }

        #endregion

    }
}