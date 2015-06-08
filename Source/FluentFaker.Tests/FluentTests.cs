using NUnit.Framework;

namespace FluentFaker.Tests
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    [TestFixture]
    public class FluentTests
    {
        [TestFixtureSetUp]
        public void BeforeRunningTestSession()
        {

            
        }

        [TestFixtureTearDown]
        public void AfterRunningTestSession()
        {

        }


        [SetUp]
        public void BeforeEachTest()
        {

        }

        [TearDown]
        public void AfterEachTest()
        {

        }


        [Test]
        public  void Test()
        {
            var x = Utils.ReplaceSymbolsWithNumbers("(###) ###-####");

            x.Dump();
        }

        
    }
}