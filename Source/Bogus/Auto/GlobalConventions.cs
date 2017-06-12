namespace Bogus.Auto
{
    public static class GlobalConventions
    {
        static GlobalConventions()
        {
            Conventions = new Conventions();
        }

        public static Conventions Conventions { get; }
    }
}
