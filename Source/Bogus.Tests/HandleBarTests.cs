using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Bogus.DataSets;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class HandleBarTests
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
        public void parse_test()
        {
            var f = new Faker();
            Tokenizer.Parse("{{name.lastName}}, {{name.firstName}} {{name.suffix}}", f.Name)
                .Dump();

        }
    }

}