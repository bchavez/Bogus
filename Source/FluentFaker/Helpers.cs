using System.Text.RegularExpressions;

namespace FluentFaker
{
    public static class Helpers
    {
        public static string Slugify(string txt)
        {
            var str = txt.Replace(" ", "");
            return Regex.Replace(str, @"[^\w\.\-]+", "");
        }
    }
}