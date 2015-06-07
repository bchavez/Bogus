using NUnit.Framework;

namespace FluentFaker.Tests
{
    public class ConsistentTest
    {
        [SetUp]
        public void BeforeEachTest()
        {
            //set the random gen manually to a seeded value
            Random.Generator = new System.Random(3116);
        }
    }
}