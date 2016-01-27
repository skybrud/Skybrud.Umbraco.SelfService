using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.SelfService.Install {

    internal class SelfServiceContentType {

        public Guid Guid { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsContainer { get; set; }
        public SelfServicePropertyGroup[] PropertyGroups { get; set; }
        public string[] AllowedContentTypes { get; set; }

        public SelfServiceContentType(string guid, string alias, string name, string icon) {
            Guid = Guid.Parse(guid);
            Alias = alias;
            Name = name;
            Icon = icon;
            PropertyGroups = new SelfServicePropertyGroup[0];
        }

        public IContentType GetOrCreate(int parentId, List<string> log) {

            log.Add("Creating content type \"" + Name + "\" (GUID: " + Guid + ")");

            // Get a reference to the content type service
            var cts = ApplicationContext.Current.Services.ContentTypeService;
            var dts = ApplicationContext.Current.Services.DataTypeService;

            // Attempt to get the content type
            IContentType contentType = cts.GetContentType(Guid);

            // Or create a new one
            if (contentType == null) {
                contentType = new ContentType(parentId) { Alias = Alias, Key = Guid, Name = Name, Icon = Icon };
                cts.Save(contentType);
                log.Add(" > Created with ID " + contentType.Id);
            } else {
                log.Add(" > Already exists with ID " + contentType.Id);
            }

            foreach (SelfServicePropertyGroup propertyGroup in PropertyGroups) {

                // Attempt to get the property group
                var pg = contentType.PropertyGroups.FirstOrDefault(x => x.Name == propertyGroup.Name);

                // Create the property group if not already present
                if (pg == null) {
                    log.Add(" > Added property group \"" + propertyGroup.Name + "\"");
                    contentType.AddPropertyGroup(propertyGroup.Name);
                    pg = contentType.PropertyGroups.First(x => x.Name == propertyGroup.Name);
                } else {
                    log.Add(" > Property group \"" + propertyGroup.Name + "\" already exists");
                }

                foreach (SelfServiceProperty property in propertyGroup.Properties) {

                    // Continue if the property already exists
                    if (pg.PropertyTypes.Any(x => x.Alias == property.Alias)) {
                        log.Add(" > Property \"" + property.Name + "\" already exists (alias: " + property.Alias + ")");
                        continue;
                    }

                    // Get the data type definition
                    IDataTypeDefinition dtd = dts.GetDataTypeDefinitionById(property.Guid);

                    // Initialize a new property type
                    PropertyType type = new PropertyType(dtd, property.Alias) {
                        Name = property.Name,
                        Description = property.Description ?? "",
                        SortOrder = pg.PropertyTypes.Count()
                    };

                    // Add the property to the property group
                    pg.PropertyTypes.Add(type);

                    log.Add(" > Added property \"" + property.Name + "\" (alias: " + property.Alias + ")");

                }

            }

            // Set whether the content type is a container
            contentType.IsContainer = IsContainer;

            // Set the allowed content types
            if (AllowedContentTypes != null) {
                int i = 0;
                contentType.AllowedContentTypes = (
                    from allowedTypeGuid in AllowedContentTypes
                    select cts.GetContentType(Guid.Parse(allowedTypeGuid))
                    into allowedType
                    where allowedType != null
                    select new ContentTypeSort(allowedType.Id, i++)
                ).ToList();
            }

            // Save the content type
            cts.Save(contentType);

            return contentType;

        }

    }

}