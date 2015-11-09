namespace Bogus.Extensions.Denmark
{
    public static class ExtensionsForDenmark
    {
        public static string Cpr(this Person p)
        {
            var r = new Randomizer();
            return $"{p.DateOfBirth:ddMMyy}-{r.Replace("####")}";
        }
    }
}