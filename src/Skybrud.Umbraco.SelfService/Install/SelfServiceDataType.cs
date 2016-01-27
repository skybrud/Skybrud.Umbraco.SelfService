using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.SelfService.Install {
    
    internal class SelfServiceDataType {
        
        public Guid Guid { get; private set; }
        public string EditorAlias { get; private set; }
        public string Name { get; private set; }

        public IDictionary<string, PreValue> PreValues { get; set; } 
        
        public SelfServiceDataType(string guid, string alias, string name) {
            Guid = Guid.Parse(guid);
            EditorAlias = alias;
            Name = name;
        }

        public bool GetOrCreate() {
            IDataTypeDefinition dtd;
            return GetOrCreate(out dtd);
        }

        public bool GetOrCreate(out IDataTypeDefinition dtd) {

            // Get a reference to the data type service
            IDataTypeService dts = ApplicationContext.Current.Services.DataTypeService;

            // Attempt to get the DTD
            dtd = dts.GetDataTypeDefinitionById(Guid);
            if (dtd != null) {
                return false;
            }

            // Or create a new one
            dtd = new DataTypeDefinition(EditorAlias) { Key = Guid, Name = Name };


            if (PreValues == null) {
                dts.Save(dtd);
            } else {
                dts.SaveDataTypeAndPreValues(dtd, PreValues);
            }

            return true;

        }

    }

}