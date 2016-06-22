using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.SelfService.Extensions;
using Umbraco.Core.Logging;

namespace Skybrud.Umbraco.SelfService.Models.List {
    
    public class SelfServiceList {

        #region Properties

        /// <summary>
        /// Gets a reference to the <see cref="JObject"/> the list was parsed from.
        /// </summary>
        [JsonIgnore]
        public JObject JObject { get; private set; }

        /// <summary>
        /// Gets an array of each <see cref="SelfServiceListItem"/> making up the list.
        /// </summary>
        [JsonProperty("items")]
        public SelfServiceListItem[] Items { get; private set; }

        /// <summary>
        /// Gets whether the list has any items.
        /// </summary>
        [JsonIgnore]
        public bool HasItems {
            get { return Items.Length > 0;  }
        }

        #endregion

        #region Constructors

        private SelfServiceList(JObject obj) {
            JObject = obj;
            Items = obj.GetArrayItems("items", SelfServiceListItem.Parse).Where(x => !String.IsNullOrWhiteSpace(x.Value)).ToArray();
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Gets a new instance of <see cref="SelfServiceList"/> based on an empty list.
        /// </summary>
        /// <returns></returns>
        public static SelfServiceList GetEmptyModel() {
            return Parse(JObject.Parse("{items:[]}"));
        }

        /// <summary>
        /// Parses the specified <see cref="JObject"/> into an instance of <see cref="SelfServiceList"/>.
        /// </summary>
        /// <param name="obj">An instance of <see cref="JObject"/> representing the list.</param>
        /// <returns>Returns an instance of <see cref="SelfServiceList"/>.</returns>
        public static SelfServiceList Parse(JObject obj) {
            return obj == null ? null : new SelfServiceList(obj);
        }

        /// <summary>
        /// Deserialize the specified <code>str</code> into an instance of <see cref="SelfServiceList"/>. If the string
        /// is empty or the deserialization triggers an exception, the value of the <see cref="GetEmptyModel"/> method
        /// will be returned instead.
        /// </summary>
        /// <param name="str">The JSON string to be parsed.</param>
        /// <returns>Returns an instance of <see cref="SelfServiceList"/>.</returns>
        public static SelfServiceList Deserialize(string str) {
            try {
                if (!String.IsNullOrWhiteSpace(str) && str.StartsWith("{")) {
                    return Parse(JObject.Parse(str));
                }
                return GetEmptyModel();
            } catch (Exception ex) {
                LogHelper.Error<SelfServiceList>("Unable to parse selfservice list value", ex);
                return GetEmptyModel();
            }
        }

        #endregion

    }

}