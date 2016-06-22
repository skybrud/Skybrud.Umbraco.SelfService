using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.SelfService.Extensions;

namespace Skybrud.Umbraco.SelfService.Models.List {

    public class SelfServiceListItem {

        #region Properties

        /// <summary>
        /// Gets a reference to the <see cref="JObject"/> the item was parsed from.
        /// </summary>
        [JsonIgnore]
        public JObject JObject { get; private set; }

        /// <summary>
        /// Gets the value of the item.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; private set; }

        #endregion

        #region Constructors

        private SelfServiceListItem(JObject obj) {
            JObject = obj;
            Value = obj.GetString("value");
        }

        #endregion

        /// <summary>
        /// Parses the specified <see cref="JObject"/> into an instance of <see cref="SelfServiceListItem"/>.
        /// </summary>
        /// <param name="obj">An instance of <see cref="JObject"/> representing the item.</param>
        /// <returns>Returns an instance of <see cref="SelfServiceListItem"/>.</returns>
        public static SelfServiceListItem Parse(JObject obj) {
            return obj == null ? null : new SelfServiceListItem(obj);
        }

    }

}