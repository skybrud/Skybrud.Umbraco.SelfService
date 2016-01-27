using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.SelfService.Install {

    public class SelfServiceInstaller {

        internal SelfServiceDataType[] DataTypes = {
            new SelfServiceDataType("83686faf-084b-4385-99e1-5b6ad4bce405", "Skybrud.SelfService.ActionPagesList", "# Selvbetjening - Handlingssider"),
            new SelfServiceDataType("97f45805-9fab-4bf9-8b2a-6130618ebc0c", "Skybrud.SelfService.Categories", "# Selvbetjening - Kategorivælger"),
            new SelfServiceDataType("df042227-bc3d-4b5c-a2a6-541c9fa12cf7", "Skybrud.SelfService.List", "# Selvbetjening - Punktopstilling") {
                PreValues = new Dictionary<string, PreValue> {
                    {"orderedList", new PreValue("1")},
                    {"maxItems", new PreValue("0")}
                }
            }
        };

        internal SelfServiceContentType[] ContentTypes = {
            new SelfServiceContentType("27534400-2583-4906-b384-0c1c487147bb", "SkySelfServiceContainer", "Self Service Container", "icon-folder"),
            new SelfServiceContentType("3d4caf63-87f2-47f2-a2e3-359eeefceeb1", "SkySelfServiceModule", "Selvbetjeningsmodul", "icon-help"),
            new SelfServiceContentType("d53ecdee-34b5-4a29-bd18-09b383e202bb", "SkySelfServiceCategory", "Selvbetjening - Kategori", "icon-tag") {
                AllowedContentTypes = new [] {
                    "d53ecdee-34b5-4a29-bd18-09b383e202bb"
                }
            },
            new SelfServiceContentType("cb3ff909-3fb5-4884-8d66-65702510582b", "SkySelfServiceCategories", "Selvbetjening - Kategorier", "icon-tags") {
                AllowedContentTypes = new [] {
                    "d53ecdee-34b5-4a29-bd18-09b383e202bb"
                }
            },
            new SelfServiceContentType("f0daad81-19b5-4b83-98e7-6a46800afec2", "SkySelfServiceActionPage", "Selvbetjening - Handlingsside", "icon-umb-content") {
                PropertyGroups = new [] {
                    new SelfServicePropertyGroup {
                        Name = "Handlingsside",
                        Properties = new [] {
                            new SelfServiceProperty("df042227-bc3d-4b5c-a2a6-541c9fa12cf7", "skySelfServicePrerequisites", "Det får du brug for", "En kort liste over de forudsætninger for, at borgeren kan udføre selvbetjeningen. Fx skal borgeren have sit nøgekort klar, hvis selvbetjeningen kræver NemId."),
                            new SelfServiceProperty("df042227-bc3d-4b5c-a2a6-541c9fa12cf7", "skySelfServiceHowTo", "Sådan gør du", "En hurtig trinbaseret forklaring af hvordan selvbetjeningen gennemføres."),
                            new SelfServiceProperty("97f45805-9fab-4bf9-8b2a-6130618ebc0c", "skySelfServiceCategories", "Kategorier", "Angiv den eller de kategorier, der bedst beskriver siden i selvbetjeningsmodulet.")
                        }
                    } 
                }
            },
            new SelfServiceContentType("4b877113-0005-49d0-86fc-45bfb604ca19", "SkySelfServiceActionPages", "Selvbetjening - Handlingssider", "icon-list") {
                IsContainer = true,
                AllowedContentTypes = new [] {
                    "f0daad81-19b5-4b83-98e7-6a46800afec2"
                }
            }
        };

        public void Run(int parentId, List<string> log) {
            SetupDataTypes(log);
            SetupContentTypes(log);
            SetupContent(parentId, log);
        }

        private void SetupDataTypes(List<string> log) {

            // Create the data types
            foreach (SelfServiceDataType dtd in DataTypes) {

                log.Add("Creating data type \"" + dtd.Name + "\" (GUID: " + dtd.Guid + ")");

                IDataTypeDefinition idtd;
                if (dtd.GetOrCreate(out idtd)) {
                    log.Add(" > Created with ID " + idtd.Id);
                } else {
                    log.Add(" > Already exists with ID " + idtd.Id);
                }

                dtd.GetOrCreate();
            }

        }

        private void SetupContentTypes(List<string> log) {
            
            // Create the content types
            IContentType container = ContentTypes[0].GetOrCreate(-1, log);
            IContentType module = ContentTypes[1].GetOrCreate(container.Id, log);
            for (int i = 2; i < ContentTypes.Length; i++) {
                ContentTypes[i].GetOrCreate(container.Id, log);
            }
        
        }

        private void SetupContent(int parentId, List<string> log) {

            IContentTypeService cts = ApplicationContext.Current.Services.ContentTypeService;
            IContentService cs = ApplicationContext.Current.Services.ContentService;

            var contentDefinitions = new[] {
                new { parent = default(string), guid = "a3f0120e-ea17-4a6d-8355-2a66ccdb2cff", type = "SkySelfServiceModule", name = "Selvbetjening" },
                new { parent = "a3f0120e-ea17-4a6d-8355-2a66ccdb2cff", guid = "42895027-98a3-49c9-845d-493abb34d5f9", type = "SkySelfServiceCategories", name = "Kategorier" },
                new { parent = "a3f0120e-ea17-4a6d-8355-2a66ccdb2cff", guid = "942daec4-34cc-4114-9f9e-37c84c6de572", type = "SkySelfServiceActionPages", name = "Handlingssider" }
            };

            foreach (var contentDefinition in contentDefinitions) {

                log.Add("Creating content node \"" + contentDefinition.name + "\"");

                Guid guid = Guid.Parse(contentDefinition.guid);

                IContent content = cs.GetById(guid);
                if (content != null) {
                    log.Add(" > Already exists with ID " + content.Id + " (GUID: " + content.Key + ")");
                    continue;
                }

                int pid = parentId;
                if (contentDefinition.parent != null) {
                    var parent = cs.GetById(Guid.Parse(contentDefinition.parent));
                    log.Add(" > Parent with GUID " + contentDefinition.parent + "not found");
                    pid = parent.Id;
                }

                IContentType contentType = cts.GetContentType(contentDefinition.type);

                content = new Content(contentDefinition.name, pid, contentType);
                content.Key = guid;
                cs.SaveAndPublishWithStatus(content);

                log.Add(" > Created with ID " + content.Id + " (GUID: " + content.Key + ")");

            }
            
        }

    }

}