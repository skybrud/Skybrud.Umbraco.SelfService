using System.Collections;
using System.Collections.Generic;
using Skybrud.Umbraco.SelfService.Models.Plugins;

namespace Skybrud.Umbraco.SelfService.Plugins {

    /// <summary>
    /// Collection used for keeping track of the added plugins.
    /// </summary>
    public class SelfServicePluginCollection : IEnumerable<SelfServicePluginBase> {

        #region Private fields

        private readonly List<SelfServicePluginBase> _list = new List<SelfServicePluginBase>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the amount of plugins added to the collection.
        /// </summary>
        public int Count {
            get { return _list.Count; }
        }

        #endregion

        #region Constructors

        internal SelfServicePluginCollection() { }

        #endregion

        #region Member methods

        /// <summary>
        /// Removes all plugins.
        /// </summary>
        public void Clear() {
            _list.Clear();
        }

        /// <summary>
        /// Adds the specified <code>plugin</code> to the end of the collection.
        /// </summary>
        /// <param name="plugin">The plugin top be added.</param>
        public void Add(SelfServicePluginBase plugin) {
            _list.Add(plugin);
        }

        /// <summary>
        /// Adds the specified <code>plugin</code> to at <code>index</code> of the collection.
        /// </summary>
        /// <param name="plugin">The plugin top be added.</param>
        /// <param name="index">The index the plugin should be added at.</param>
        public void AddAt(SelfServicePluginBase plugin, int index) {
            _list.Insert(index, plugin);
        }

        public IEnumerator<SelfServicePluginBase> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

    }

}