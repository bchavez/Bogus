using System;
using System.Linq;
using Bogus.DataSets;
using FluentAssertions;
using NUnit.Framework;
using Z.ExtensionMethods;

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

            avatar.Should().Be("https://s3.amazonaws.com/uifaces/faces/twitter/nasirwd/128.jpg");
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
        public void can_get_a_random_ipv6_address()
        {
            internet.Ipv6().Should().Be("da23:9c4c:e0c4:2dd7:e3c4:a896:17f2:55b2");
        }

        [Test]
        public void can_get_html_color()
        {
            internet.Color().Should().Be("#4d0e68");
        }

        [Test]
        public void can_generate_mac_address()
        {
            internet.Mac().Should().Be("9a:1c:d0:a5:09:9f");
        }

        [Test]
        public void can_generate_an_example_email()
        {
            var email = internet.ExampleEmail();
            
            email.Should().EndWith("@example.com");
            email.GetBefore("@").Should().Contain(".");
        }

        [Test]
        public void can_generate_a_password()
        {
            var pw = internet.Password();
            pw.Should().Be("YmaMy0eWbv");

            var pw2 = internet.Password(regexPattern: @"\W");
            pw2.Should().Be(@""">({=*`/{]");
        }


        [Test]
        public void can_pick_random_browser()
        {
            Enumerable.Range(1, 200).Select(
                    i => internet.UserAgent())
                .Dump();
        }

        [Test]
        public void can_get_url_with_path()
        {
            internet.UrlWithPath().Should().Be("https://ambrose.net/soft/deposit");
        }

        [Test]
        public void can_get_a_urlpath_with_a_specific_domain()
        {
            internet.UrlWithPath( domain: "bitarmory.com").Should().Be("https://bitarmory.com/soft/deposit");
        }
    }
}
