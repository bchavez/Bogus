using System.Linq;
using Bogus.DataSets;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   //https://github.com/bchavez/Bogus/issues/99
   public class Issue99 : SeededTest
   {
      [Fact]
      public void multi_threaded_locale_access_should_be_okay()
      {
         ParallelEnumerable.Range(0, 9999)
            .Select(i => new Name("nl")).ToList();
      }
   }
}