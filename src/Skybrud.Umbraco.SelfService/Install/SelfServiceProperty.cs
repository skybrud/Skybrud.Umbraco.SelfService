using System;

namespace Skybrud.Umbraco.SelfService.Install {
    
    internal class SelfServiceProperty {
        
        public Guid Guid { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public SelfServiceProperty(string guid, string alias, string name) {
            Guid = Guid.Parse(guid);
            Alias = alias;
            Name = name;
        }

        public SelfServiceProperty(string guid, string alias, string name, string description) {
            Guid = Guid.Parse(guid);
            Alias = alias;
            Name = name;
            Description = description;
        }
    
    }

}