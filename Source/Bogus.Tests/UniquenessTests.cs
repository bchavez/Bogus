using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Z.ExtensionMethods;
using Z.ExtensionMethods.ObjectExtensions;

namespace Bogus.Tests
{
    public class UniquenessTests : SeededTest
    {
        public class User
        {
            public string FirstName{get; set;}
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
        }

        [Test]
        public void every_new_generation_should_have_a_new_unqiue_index()
        {
            Faker.GlobalUniqueIndex = 0;
            var faker = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Username, f => f.UniqueIndex + f.Person.UserName);

            var fakes = faker.Generate(10).ToList();

            fakes.Dump();

            faker.FakerHub.UniqueIndex.Should().Be(10);

            var values = fakes
                .Select(u => u.Username.Left(1).ToInt32())
                .ToArray();

            values.Should().BeEquivalentTo(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);

            var morefakes = faker.Generate(3).ToList();

            morefakes.Dump();

            faker.FakerHub.UniqueIndex.Should().Be(13);
        }

        public class Video
        {
            public string VideoId { get; set; }
            public string Summary { get; set; }
        }

        [Test]
        public void should_be_able_to_create_some_hash_ids()
        {
            var faker = new Faker<Video>()
                .RuleFor(v => v.VideoId, f => f.Hashids.EncodeLong(f.UniqueIndex))
                .RuleFor(v => v.Summary, f => f.Lorem.Sentence());

            var fakes = faker.Generate(5).ToList();

            fakes.Dump();

            var ids = fakes.Select(v => v.VideoId).ToArray();

            ids.Should().BeEquivalentTo("gY", "jR", "k5", "l5", "mO");
        }

    }
}