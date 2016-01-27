using System;
using System.Linq;
using Examine;
using Newtonsoft.Json;
using Skybrud.Umbraco.SelfService.Models.Categories;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.SelfService.Models.ActionPages {
    
    public class SelfServiceActionPage {

        #region Properties

        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("categories")]
        public SelfServiceCategory[] Categories { get; private set; }

        #endregion

        #region Member methods

        public SelfServiceCategory[] GetCategories(Func<SelfServiceCategory, bool> predicate) {
            return Categories.Where(predicate).ToArray();
        }

        #endregion

        private SelfServiceActionPage(SearchResult result, IPublishedContent content) {

            Id = result.Id;

            if (content == null) {

                string title;
                Title = result.Fields.TryGetValue("title", out title) ? title : result.Fields["nodeName"];

                Categories = new SelfServiceCategory[0];

            } else {

                Title = content.HasValue("title") ? content.GetPropertyValue<string>("title") : content.Name;

                Url = content.Url;

                Categories = content.GetPropertyValue<SelfServiceCategory[]>("skySelfServiceCategories") ?? new SelfServiceCategory[0];

            }
        
        }

        public static SelfServiceActionPage GetFromResult(SearchResult result) {
            if (result == null) return null;
            IPublishedContent content = (UmbracoContext.Current == null ? null : UmbracoContext.Current.ContentCache.GetById(result.Id));
            return new SelfServiceActionPage(result, content);
        }

    }

}