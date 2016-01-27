using System.Web;
using Skybrud.Umbraco.SelfService.Models.ActionPages;
using Skybrud.Umbraco.SelfService.Models.Categories;
using Skybrud.Umbraco.SelfService.Plugins;

namespace Skybrud.Umbraco.SelfService {

    public class SelfServiceContext {

        #region Private fields

        private static readonly SelfServicePluginCollection PluginCollection = new SelfServicePluginCollection();

        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the current self service context. The context lives for the duration of the current
        /// page cycle.
        /// </summary>
        public static SelfServiceContext Current {
            get {
                SelfServiceContext current = HttpContext.Current.Items["SelfServiceContext"] as SelfServiceContext;
                if (current == null) {
                    HttpContext.Current.Items["SelfServiceContext"] = current = new SelfServiceContext();
                }
                return current;
            }
        }

        /// <summary>
        /// Gets a collection of all plugins. Even though the <code>SelfServiceContext</code> only lives across a
        /// single, this property will live during the lifetime of the application.
        /// </summary>
        public SelfServicePluginCollection Plugins {
            get { return PluginCollection; }
        }

        /// <summary>
        /// Gets a reference to the categories repository.
        /// </summary>
        public SelfServiceCategoriesRepository Categories { get; private set; }

        /// <summary>
        /// Gets a reference to the action pages repository.
        /// </summary>
        public SelfServiceActionPagesRepository ActionPages { get; private set; }

        #endregion

        #region Constructors

        private SelfServiceContext() {
            Categories = new SelfServiceCategoriesRepository(this);
            ActionPages = new SelfServiceActionPagesRepository(this);
        }

        #endregion

    }

}