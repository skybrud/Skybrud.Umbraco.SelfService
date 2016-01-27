using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.SelfService.Models.Categories {
    
    /// <summary>
    /// Class representing at category in the self service database.
    /// </summary>
    public class SelfServiceCategory {

        #region Private fields

        private SelfServiceCategory[] _children;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the underlying <code>IPublishedContent</code>.
        /// </summary>
        [JsonIgnore]
        public IPublishedContent Content { get; private set; }

        /// <summary>
        /// Gets the ID of the category.
        /// </summary>
        [JsonProperty("id")]
        public int Id {
            get { return Content.Id; }
        }

        [JsonIgnore]
        public int Level {
            get { return Content.Level - 3; }
        }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        [JsonProperty("name")]
        public string Name {
            get { return Content.HasValue("title") ? Content.GetPropertyValue<string>("title") : Content.Name; }
        }

        /// <summary>
        /// Gets a reference to the parent category, or <code>null</code> if the category doesn't have a parent.
        /// </summary>
        [JsonIgnore]
        public SelfServiceCategory Parent { get; private set; }

        /// <summary>
        /// Gets an array of all child categories.
        /// </summary>
        [JsonIgnore]
        public SelfServiceCategory[] Children {
            get {
                return _children ?? (_children = Content.Children.Select(child => SelfServiceContext.Current.Categories.ParseCategory(child, this)).ToArray());
            }
        }

        #endregion

        #region Constructors

        protected SelfServiceCategory(IPublishedContent content, SelfServiceCategory parent = null) {
            Content = content;
            Parent = parent;
        }

        #endregion

        #region Member methods

        public SelfServiceCategory AncestorOrSelf(int level) {
            SelfServiceCategory parent = this;
            while (parent != null) {
                if (parent.Level == level) return parent;
                parent = parent.Parent;
            }
            return null;
        }

        #endregion

        #region Static methods

        public static SelfServiceCategory GetFromContent(IPublishedContent content, SelfServiceCategory parent = null) {
            return content == null ? null : new SelfServiceCategory(content, parent);
        }

        #endregion

    }

}