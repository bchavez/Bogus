using Bogus.Tests.Models;
using FluentAssertions;
using System;
using Xunit;

namespace Bogus.Tests.GitHubIssues;

public class PullRequest500 : SeededTest
{
   public class PersonTest
   {
      public string Name { get; set; }
      public DateTime Birhtday { get; set; }
   }

   [Fact]
   public void can_set_and_use_datetimereference_on_Faker()
   {
      var expected = new System.DateTime(2017, 11, 11, 11, 11, 11);

      var f = new Faker()
      {
         Random = new Randomizer(7331),
         DateTimeReference = expected
      };

      f.Date.LocalSystemClock().Should().Be(expected);
   }


   [Fact]
   public void can_set_and_use_datetimereference_on_FakerT()
   {
      var expected = new System.DateTime(2017, 11, 11, 11, 11, 11);

      var f = new Faker<Order>();
      f.UseSeed(7331);
      f.UseDateTimeReference(expected);

      f.localDateTimeRef.Should().Be(expected);

      var internals = f as IFakerTInternal;
      internals.FakerHub.DateTimeReference.Should().NotBeNull();
      internals.FakerHub.Date.LocalSystemClock().Should().Be(expected);
   }


   [Fact]
   public void cloning_FakerT_still_uses_datetimereference_on_source_FakerT()
   {
      var expected = new System.DateTime(2017, 11, 11, 11, 11, 11);

      var fakerT = new Faker<Order>();
      fakerT.UseSeed(7331);
      fakerT.UseDateTimeReference(expected);

      var clone = fakerT.Clone();

      clone.localDateTimeRef.Should().Be(expected);

      var cloneInternals = clone as IFakerTInternal;
      cloneInternals.FakerHub.DateTimeReference.Should().NotBeNull();
      cloneInternals.FakerHub.Date.LocalSystemClock().Should().Be(expected);
   }

   [Fact]
   public void null_usedatetimereference_reverts_to_systemclock_FakerT()
   {
      var expected = new System.DateTime(2017, 11, 11, 11, 11, 11);

      var fakerT = new Faker<Order>();
      fakerT.UseSeed(7331);
      fakerT.UseDateTimeReference(expected);
      fakerT.UseDateTimeReference(null);

      fakerT.localDateTimeRef.Should().BeNull();

      var internals = fakerT as IFakerTInternal;
      internals.FakerHub.DateTimeReference.Should().BeNull();
      internals.FakerHub.Date.LocalSystemClock.Should().BeNull();
   }


   [Fact]
   public void null_datetimereference_should_use_systemclock_for_Faker()
   {
      var f = new Faker()
      {
         Random = new Randomizer(7331),
         DateTimeReference = new System.DateTime(2017, 11, 11, 11, 11, 11)
      };

      f.DateTimeReference = null;

      f.Date.LocalSystemClock.Should().BeNull();
   }


   [Fact]
   public void FakerT_usedatetimereference_flows_to_Person()
   {
      var expected = new System.DateTime(2017, 11, 11, 11, 11, 11);

      var f = new Faker<PersonTest>()
         .UseSeed(7331)
         .UseDateTimeReference(expected)
         .RuleFor(f => f.Name, f => f.Person.FirstName + " " + f.Person.LastName)
         .RuleFor(f => f.Birhtday, f => f.Date.Recent());

      var internals = f as IFakerTInternal;
      internals.FakerHub.Person.DsDate.LocalSystemClock().Should().Be(expected);

      var p = f.Generate();
      p.Birhtday.Should().BeCloseTo(expected, precision: TimeSpan.FromDays(1));
   }

   [Fact]
   public void using_Person_refDate_sets_dsdate_localsystemclock()
   {
      var expected = new System.DateTime(2017, 11, 11, 11, 11, 11);
      var p = new Bogus.Person(refDate: expected);
      p.DsDate.LocalSystemClock.Should().NotBeNull();
      p.DsDate.LocalSystemClock().Should().Be(expected);
   }


   [Fact]
   public void using_internal_Person_refDate_sets_dsdate_localsystemclock()
   {
      var expected = new System.DateTime(2017, 11, 11, 11, 11, 11);
      var p = new Bogus.Person(new Randomizer(), refDate: expected);
      p.DsDate.LocalSystemClock.Should().NotBeNull();
      p.DsDate.LocalSystemClock().Should().Be(expected);

      p.DateOfBirth.Should()
         .BeOnOrBefore(expected.AddYears(-20))
         .And
         .BeOnOrAfter(expected.AddYears(-20 + -50));
   }

}
