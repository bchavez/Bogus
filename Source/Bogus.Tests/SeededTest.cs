using Xunit;

namespace Bogus.Tests
{
    [Collection("Seeded Test")]
    public class SeededTest
    {
        public SeededTest()
        {
            //set the random gen manually to a seeded value
            Randomizer.Seed = new System.Random(3116);
        }
    }
}