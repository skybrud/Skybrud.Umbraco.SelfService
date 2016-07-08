using System.Collections.Generic;
using System.Linq;
using Skybrud.Umbraco.SelfService.Constants;
using Skybrud.Umbraco.SelfService.Helpers;
using Skybrud.Umbraco.SelfService.Models.Plugins;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.SelfService.Models.Categories {

    /// <summary>
    /// Repository class for working with categories.
    /// </summary>
    public class SelfServiceCategoriesRepository {

        #region Private fields

        private SelfServiceCategory[] _categories;
        private Dictionary<int, SelfServiceCategory> _categoriesLookup;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the parent self service context.
        /// </summary>
        public SelfServiceContext Context { get; private set; }

        /// <summary>
        /// Gets an array of all categories.
        /// </summary>
        public SelfServiceCategory[] All {
            get {
                if (_categories == null) { LoadCategories(); }
                return _categoriesLookup.Values.ToArray();
            }
        }

        #endregion

        #region Constructors

        internal SelfServiceCategoriesRepository(SelfServiceContext context) {
            Context = context;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Gets an array of the top most categories.
        /// </summary>
        /// <returns>Returns an array of <code>SelfServiceCategory</code>.</returns>
        public SelfServiceCategory[] GetCategories() {
            if (_categories == null) { LoadCategories(); }
            return _categories;
        }

        /// <summary>
        /// Gets the category with the specified <code>categoryId</code>, or <code>null</code> if not found.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>Returns an instance of <code>SelfServiceCategory</code>, or <code>null</code> if the category wasn't found.</returns>
        public SelfServiceCategory GetCategoryById(int categoryId) {
            if (_categories == null) { LoadCategories(); }
            SelfServiceCategory category;
            return _categoriesLookup.TryGetValue(categoryId, out category) ? category : null;
        }

        /// <summary>
        /// Gets an array of categories based on the specified <code>categoryIds</code>.
        /// </summary>
        /// <param name="categoryIds">A string of category IDs separate by either commas or spaces.</param>
        /// <returns>Returns an array of <code>SelfServiceCategory</code>.</returns>
        public SelfServiceCategory[] GetCategoryByIds(string categoryIds) {
            return GetCategoryByIds(StringHelpers.CsvToInt(categoryIds));
        }

        /// <summary>
        /// Gets an array of categories based on the specified <code>categoryIds</code>.
        /// </summary>
        /// <param name="categoryIds">An array of category IDs.</param>
        /// <returns>Returns an array of <code>SelfServiceCategory</code>.</returns>
        public SelfServiceCategory[] GetCategoryByIds(params int[] categoryIds) {
            if (_categories == null) { LoadCategories(); }
            categoryIds = categoryIds ?? new int[0];
            return categoryIds.Select(GetCategoryById).Where(category => category != null).ToArray();
        }

        /// <summary>
        /// Loads all categories.
        /// </summary>
        private void LoadCategories() {

            // Get the first level of categories
            IEnumerable<IPublishedContent> level1 = UmbracoContext.Current.ContentCache.GetByXPath(SelfServiceConstants.Categories1LevelXPath);

            // Parse the categories (children are parsed recursively)
            _categories = level1.Select(ParseCategory).ToArray();

            // Init the lookup dictionary
            _categoriesLookup = new Dictionary<int, SelfServiceCategory>();

            // Start building the lookup (recursively)
            BuildCategoriesLookup(_categoriesLookup, _categories);

        }

        /// <summary>
        /// Recursive method for building a dictionary for fast lookups based on the ID of each category.
        /// </summary>
        /// <param name="dictionary">The dictionary to which the categories should be added.</param>
        /// <param name="categories">A collection of categories to be added to the dictionary.</param>
        private void BuildCategoriesLookup(Dictionary<int, SelfServiceCategory> dictionary, IEnumerable<SelfServiceCategory> categories) {
            foreach (SelfServiceCategory category in categories) {
                dictionary.Add(category.Id, category);
                BuildCategoriesLookup(dictionary, category.Children);
            }
        }

        /// <summary>
        /// Method used for parsing an instance if <code>IPublishedContent</code> into an instance of <code>SelfServiceCategory</code>.
        /// </summary>
        /// <param name="content">The instance of <code>IPublishedContent</code> representing the category.</param>
        /// <returns>Returns an instance of <code>SelfServiceCategory</code> representing the catrgory.</returns>
        public SelfServiceCategory ParseCategory(IPublishedContent content) {
            return ParseCategory(content, null);
        }

        /// <summary>
        /// Method used for parsing an instance if <code>IPublishedContent</code> into an instance of <code>SelfServiceCategory</code>.
        /// </summary>
        /// <param name="content">The instance of <code>IPublishedContent</code> representing the category.</param>
        /// <param name="parent">The parent category, or <code>null</code> if no parent category.</param>
        /// <returns>Returns an instance of <code>SelfServiceCategory</code> representing the catrgory.</returns>
        public SelfServiceCategory ParseCategory(IPublishedContent content, SelfServiceCategory parent) {

            // If "content" is NULL, we don't bother doing any further parsing
            if (content == null) return null;

            // Iterate through the added plugins
            foreach (SelfServicePluginBase plugin in Context.Plugins) {

                // Let the plugin parse the category (NULL means the plugin didn't handle or parse the category)
                SelfServiceCategory category = plugin.ParseCategory(content, parent);
                if (category != null) return category;

            }

            // Fallback if no plugins did handle or parse the category
            return SelfServiceCategory.GetFromContent(content, parent);

        }

        #endregion

    }

}