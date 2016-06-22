using Skybrud.Umbraco.SelfService.Models.List;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.SelfService {

    internal class SelfServicePropertyValueConverter : IPropertyValueConverter {

        public bool IsConverter(PublishedPropertyType propertyType) {
            return (
                propertyType.PropertyEditorAlias == "Skybrud.SelfService.Categories"
                ||
                propertyType.PropertyEditorAlias == "Skybrud.SelfService.List"
            );
        }

        public object ConvertDataToSource(PublishedPropertyType propertyType, object data, bool preview) {
            switch (propertyType.PropertyEditorAlias) {
                case "Skybrud.SelfService.Categories":
                    return SelfServiceContext.Current.Categories.GetCategoryByIds(data as string);
                case "Skybrud.SelfService.List":
                    return SelfServiceList.Deserialize(data as string);
                default:
                    return null;
            }
        }

        public object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview) {
            return source;
        }

        public object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview) {
            return null;
        }

    }

}