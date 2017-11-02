using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   //https://github.com/bchavez/Bogus/issues/99
   public class Issue99
   {
      [Fact]
      public void multi_threaded_locale_access_should_be_okay()
      {
         ParallelEnumerable.Range(0, 9999)
            .Select(i => new Name("nl")).ToList();
      }
   }
}