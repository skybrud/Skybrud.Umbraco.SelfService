using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Skybrud.Umbraco.SelfService.Extensions {

    public static class JObjectExtensions {

        /// <summary>
        /// Gets a string from a property with the specified <code>propertyName</code>.
        /// </summary>
        /// <param name="obj">The parent object of the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        public static string GetString(this Newtonsoft.Json.Linq.JObject obj, string propertyName) {
            if (obj == null) return null;
            JToken property = obj.GetValue(propertyName);
            return property == null ? null : property.Value<string>();
        }

        /// <summary>
        /// Gets the string value from the property with the specified <code>propertyName</code> and parses it into an
        /// instance of <code>T</code> using the specified <code>callback</code>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The parent object.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="callback">The callback used for converting the string value.</param>
        public static T GetString<T>(this Newtonsoft.Json.Linq.JObject obj, string propertyName, Func<string, T> callback) {
            if (obj == null) return default(T);
            JToken property = obj.GetValue(propertyName);
            return property == null ? default(T) : callback(property.Value<string>());
        }

        /// <summary>
        /// Gets an array of <code>T</code> from a property with the specified <code>propertyName</code> using the
        /// specified delegate <code>func</code> for parsing each item in the array.
        /// </summary>
        /// <param name="obj">The parent object of the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="func">The delegate (callback method) used for parsing each item in the array.</param>
        public static T[] GetArray<T>(this JObject obj, string propertyName, Func<JObject, T> func) {

            if (obj == null) return null;

            JArray property = obj.GetValue(propertyName) as JArray;
            if (property == null) return null;

            return (
                from JObject child in property
                select func(child)
            ).ToArray();

        }

        /// <summary>
        /// Gets the items of the <see cref="JArray"/> from the token matching the specfied <code>path</code>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="String"/> that contains a JPath expression.</param>
        /// <param name="callback">A callback function used for parsing or converting the token value.</param>
        /// <returns>Returns an array of <see cref="T"/>. If the a matching token isn't found, an empty array will
        /// still be returned.</returns>
        public static T[] GetArrayItems<T>(this JObject obj, string path, Func<JObject, T> callback) {

            if (obj == null) return new T[0];

            JArray token = obj.SelectToken(path) as JArray;
            if (token == null) return new T[0];

            return (
                from JObject child in token
                select callback(child)
            ).ToArray();

        }

    }

}