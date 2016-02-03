using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.SelfService.Constants {
    
    public class SelfServiceConstants {

        //public string RootContentGuid = "d2ffdc92-86ca-4d21-b33b-07ec2e3ba05d";

        //public const string ContainerGuid = "27534400-2583-4906-b384-0c1c487147bb";
        //public const string ContainerAlias = "TestSkySelfServiceContainer";

        //public const string ModuleGuid = "3d4caf63-87f2-47f2-a2e3-359eeefceeb1";
        //public const string ModuleAlias = "TestSkySelfServiceModule";

        //public const string CategoriesGuid = "cb3ff909-3fb5-4884-8d66-65702510582b";
        //public const string CategoriesAlias = "TestSkySelfServiceCategories";

        //public const string CategoryGuid = "d53ecdee-34b5-4a29-bd18-09b383e202bb";
        //public const string CategoryAlias = "TestSkySelfServiceCategory";

        //public const string ActionPagesGuid = "4b877113-0005-49d0-86fc-45bfb604ca19";
        //public const string ActionPagesAlias = "TestSkySelfServiceActionPages";

        //public const string ActionPageGuid = "f0daad81-19b5-4b83-98e7-6a46800afec2";
        //public const string ActionPageAlias = "TestSkySelfServiceActionPage";

        public const string CategoriesXPath = "//SkyModules/SkySelfServiceModule/SkySelfServiceCategories";
        public const string Categories1LevelXPath = "//SkyModules/SkySelfServiceModule/SkySelfServiceCategories/SkySelfServiceCategory";

        public static class Pages {

            /// <summary>
            /// Gets the GUID of the module node.
            /// </summary>
            public const string Module = "a3f0120e-ea17-4a6d-8355-2a66ccdb2cff";

            /// <summary>
            /// Gets the GUID of the categories node.
            /// </summary>
            public const string Categories = "42895027-98a3-49c9-845d-493abb34d5f9";

            /// <summary>
            /// Gets the GUID of the action pages node.
            /// </summary>
            public const string ActionPages = "942daec4-34cc-4114-9f9e-37c84c6de572";
            
        }

    }

}