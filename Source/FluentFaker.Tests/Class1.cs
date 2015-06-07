using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public static class ExtensionsForJToken
    {
        public static void Dump(this JToken token)
        {
            Console.WriteLine(JsonConvert.SerializeObject(token));
        }
    }

    [TestFixture]
    public class SimpleTests
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
            var d = Faker.Get("name", "first_name");

            //d.Dump();

            var name = new Internet();

            var gen = name.Color();
            Console.WriteLine(gen);
        }

        
    }

}
