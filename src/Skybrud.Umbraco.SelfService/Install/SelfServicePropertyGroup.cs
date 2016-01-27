namespace Skybrud.Umbraco.SelfService.Install {
    
    internal class SelfServicePropertyGroup {
    
        public string Name { get; set; }
        public SelfServiceProperty[] Properties { get; set; }

        public SelfServicePropertyGroup() {
            Properties = new SelfServiceProperty[0];
        }
    
    }

}