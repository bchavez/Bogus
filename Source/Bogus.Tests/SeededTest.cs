using Xunit;

namespace Bogus.Tests
{
    /// <summary>
    /// Tests deriving from <see cref="SeededTest"/> ensures
    /// that when xunit runs, that seeded tests run one at a
    /// time in the same test collection (without parallelization)
    /// since Randomizer.Seed is a static property.
    /// </summary>
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