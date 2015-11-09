namespace Bogus.Extensions.Denmark
{
    public static class ExtensionsForDenmark
    {
        public static string Cpr(this Person p)
        {
            const string Key = nameof(ExtensionsForDenmark) + "CPR";
            if (p.context.ContainsKey(Key))
            {
                return p.context[Key] as string;
            }

            var r = new Randomizer();
            var final =  $"{p.DateOfBirth:ddMMyy}-{r.Replace("####")}";

            p.context[Key] = final;
            return final;
        }
    }
}