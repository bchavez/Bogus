using NUnit.Framework;

namespace FluentFaker.Tests
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
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
            new Faker<User>()
                .RuleFor(u => u.FirstName)
                .Use<Name>(n => n.FirstName())
                .RuleFor(u => u.UserName)
                .Use<Name>(n => n.LastName());

        }

        
    }
}