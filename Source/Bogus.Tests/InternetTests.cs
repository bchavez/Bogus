using System;
using FluentAssertions;
using Bogus.Generators;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class InternetTests : SeededTest
    {
        private Internet internet;

        [SetUp]
        public void BeforeEachTest()
        {
            internet = new Internet();
        }

        [Test]
        public void can_get_avatar()
        {
            var avatar = internet.Avatar();

            avatar.Should().Be("https://s3.amazonaws.com/uifaces/faces/twitter/demersdesigns/128.jpg");
        }

        [Test]
        public void can_get_an_email()
        {
            var email = internet.Email();

            email.Should().Be("Bernhard.Schultz@yahoo.com");
        }

        [Test]
        public void can_make_email_with_custom_options()
        {
            var email = internet.Email(provider: "x.y.z.com");
            
            email.Should().Be("Lee_Brown3@x.y.z.com");

            email = internet.Email(firstName: "cowboy");

            email.Should().Be("cowboy.Bechtelar30@yahoo.com");
        }

        [Test]
        public void can_get_an_username()
        {
            var user = internet.UserName();

            user.Should().Be("Lee_Brown3");
        }

        [Test]
        public void can_get_a_domain_name()
        {
            internet.DomainName().Should().Be("lee.com");
        }

        [Test]
        public void can_gets_a_domain_word()
        {
            internet.DomainWord().Should().Be("lee");
        }

        [Test]
        public void can_get_a_domain_suffix()
        {
            internet.DomainSuffix().Should().Be("name");
        }

        [Test]
        public void can_get_a_random_ip_address()
        {
            internet.Ip().Should().Be("154.28.208.165");
        }

        [Test]
        public void can_get_html_color()
        {
            internet.Color().Should().Be("#4d0e68");
        }
    }
}
