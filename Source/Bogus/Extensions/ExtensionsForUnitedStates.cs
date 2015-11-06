namespace Bogus.Extensions.UnitedStates
{
    public static class ExtensionsForUnitedStates
    {
        public static string Ssn(this Person p)
        {
            const string Key = nameof(ExtensionsForUnitedStates) + "SSN";

            if( p.context.ContainsKey(Key) )
            {
                return p.context[Key] as string;
            }

            var randomizer = new Randomizer();
            var ssn = randomizer.ReplaceNumbers("###-##-####");

            p.context[Key] = ssn;

            return ssn;
        }
    }
}