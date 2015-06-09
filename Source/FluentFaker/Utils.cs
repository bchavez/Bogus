using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FluentFaker
{
    public static class Utils
    {
        public static string Slugify(string txt)
        {
            var str = txt.Replace(" ", "");
            return Regex.Replace(str, @"[^\w\.\-]+", "");
        }

        internal static string FormatWith(this string format, params object[] objs)
        {
            return string.Format(format, objs);
        }
    }
}