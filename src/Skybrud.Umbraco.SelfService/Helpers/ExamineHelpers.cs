using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Skybrud.Umbraco.SelfService.Helpers {
    
    public static class ExamineHelpers {

        public static string PrepareSearchString(string query) {
            return Regex.Replace(query ?? "", @"[^\wæøåÆØÅ\- ]", "").ToLowerInvariant().Trim();
        }

        public static string GetQueryFromList(List<object> list) {

            if (list == null) return "";

            StringBuilder sb = new StringBuilder();

            foreach (object item in list) {
                if (item is string) {
                    sb.AppendLine(item + "");
                } else if (item is List<object>) {
                    sb.AppendLine("(");
                    sb.AppendLine(GetQueryFromList((List<object>)item));
                    sb.AppendLine(")");
                }
            }

            return sb.ToString();

        }

    }

}