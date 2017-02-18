using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bogus
{
    /// <summary>
    /// Some utility functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Slugify's text so that it is URL compatible. IE: "Can make food" -> "Can-make-food".
        /// </summary>
        public static string Slugify(string txt)
        {
            var str = txt.Replace(" ", "-");
            return Regex.Replace(str, @"[^\w\.\-]+", "");
        }

        internal static string FormatWith(this string format, params object[] objs)
        {
            return string.Format(format, objs);
        }
    }
}
