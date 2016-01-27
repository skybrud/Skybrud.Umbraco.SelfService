using System;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.SelfService.Helpers {

    public class PublishedContentHelpers {

        public static IPublishedContent[] TypedCsvContent(string value) {
            if (UmbracoContext.Current == null) return new IPublishedContent[0];
            return (
                from id in StringHelpers.CsvToInt(value)
                let item = UmbracoContext.Current.ContentCache.GetById(id)
                where item != null
                select item
            ).ToArray();
        }

        public static T[] TypedCsvContent<T>(string value, Func<IPublishedContent, T> func) {
            if (UmbracoContext.Current == null) return new T[0];
            return (
                from id in StringHelpers.CsvToInt(value)
                let item = UmbracoContext.Current.ContentCache.GetById(id)
                where item != null
                select func(item)
            ).ToArray();
        }

    }

}