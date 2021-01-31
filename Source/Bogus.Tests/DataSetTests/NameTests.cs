using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class NameTests : SeededTest
   {
      public NameTests()
      {
         name = new Name();
      }

      private readonly Name name;

      [Fact]
      public void can_get_first_name()
      {
         name.FirstName().Should().Be("Lee");
      }

      [Fact]
      public void can_get_female_first_name()
      {
         name.FirstName(Name.Gender.Female).Should().Be("Lindsay");
      }

      [Fact]
      public void can_get_male_first_name()
      {
         name.FirstName(Name.Gender.Male).Should().Be("Stuart");
      }

      [Fact]
      public void can_get_first_name_when_locale_dataset_is_split_in_male_female()
      {
         var n = new Name("ru");

         n.FirstName().Should().Be("Анастасия");
      }

      [Fact]
      public void can_get_last_name()
      {
         name.LastName().Should().Be("Mitchell");
      }

      [Fact]
      public void can_get_last_name_when_locale_dataset_is_split_in_male_female()
      {
         var n = new Name("ru");

         n.LastName().Should().Be("Киселева");
      }

      [Fact]
      public void can_get_prefix()
      {
         name.Prefix().Should().Be("Mr.");
      }

      [Fact]
      public void can_get_suffix()
      {
         name.Suffix().Should().Be("V");
      }

      [Fact]
      public void should_be_able_to_get_any_full_name()
      {
         var n = name.FindName();
         n.Length.Should().BeGreaterThan(4);
         n.Should().Contain(" ");
      }

      [Fact]
      public void should_be_able_to_get_any_name_with_options()
      {
         name.FindName("cowboy")
            .Should().StartWith("cowboy");

         name.FindName(lastName: "cowboy")
            .Should().EndWith("cowboy");

         name.FindName(withPrefix: false, withSuffix: false)
            .Should().Contain(" ");

         name.FindName("cowboy", withPrefix: false, withSuffix: false)
            .Should().StartWith("cowboy");

         name.FindName(lastName: "cowboy", withPrefix: false, withSuffix: false)
            .Should().EndWith("cowboy");
      }

      [Fact]
      public void should_be_able_to_get_job_area()
      {
         name.JobArea().Should().Be("Communications");
      }

      [Fact]
      public void should_be_able_to_get_job_description()
      {
         name.JobDescriptor().Should().Be("Investor");
      }

      [Fact]
      public void should_be_able_to_get_job_title()
      {
         name.JobTitle().Should().Be("Investor Research Assistant");
      }

      [Fact]
      public void should_be_able_to_get_job_type()
      {
         name.JobType().Should().Be("Orchestrator");
      }

      [Fact]
      public void should_be_able_to_get_locale_full_name()
      {
         var n = new Name("ru");
         n.FindName().Should().Be("Анастасия Евсеева");
      }

      [Fact]
      public void switch_locale_syntax()
      {
         var n = new Name("ru");
         n.LastName().Should().Be("Киселева");

         //switch to EN
         n["en"].LastName().Should().Be("Schultz");
      }

      [Fact]
      public void can_get_a_full_name()
      {
         name.FullName().Should().Be("Lee Brekke");
      }

      [Fact]
      public void full_name_component_genders_should_match()
      {
         var n = new Name("ru")
            {
               Random = new Randomizer(31337)
            };
         n.FullName().Should().Be("Людмила Тетерина");
      }

      [Fact]
      public void locales_with_empty_array_suffix_should_be_null()
      {
         var n = new Name("ru");
         n.Prefix().Should().BeNullOrEmpty();
         n.Suffix().Should().BeNullOrEmpty();

         n = new Name("it");
         n.Suffix().Should().BeNullOrEmpty();
      }

      [Fact]
      public void locales_that_dont_support_gender_first_names_should_return_generic()
      {

         var n = new Name("ge") { Random = new Randomizer(31337) };
         n.FirstName(Name.Gender.Female).Should().Be("ხირხელა");

         n = new Name("ge") { Random = new Randomizer(31337) };
         n.FirstName(Name.Gender.Male).Should().Be("ხირხელა");

         n = new Name("ge") { Random = new Randomizer(31337) };
         n.FirstName().Should().Be("ხირხელა");
      }
   }
}
