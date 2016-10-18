using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Skybrud.Umbraco.SelfService.Extensions {
    
    public static class StringExtensions {

        /// <summary>
        /// Converts a comma separated string into an array of integers.
        /// </summary>
        /// <param name="source">The comma separated string to be converted.</param>
        public static int[] CsvToInt(this string source) {
            return (
                from piece in (source ?? "").Split(new[] { ',', ' ', ';', ':', '|' }, StringSplitOptions.RemoveEmptyEntries)
                where Regex.IsMatch(piece, "^(-|)[0-9]+$")
                select Int32.Parse(piece)
            ).ToArray();
        }

    }

}