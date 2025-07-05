using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
    public class Issue601 : SeededTest
    {
       [Fact]
       public void can_generate_custom_avatar_ipfs_url()
       {
          var f = new Faker();
          f.Internet.Avatar().Should().StartWith("https://ipfs.io/ipfs");
          f.Internet.Avatar("https://foobar").Should().StartWith("https://foobar/ipfs");
       }
    }
}
