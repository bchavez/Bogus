using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class GitHubIssues : SeededTest
    {
        public class Bar
        {
            public int Id { get; set; }
            public string Name;
            public string Email { get; set; }
            internal string LastName;
        }

        [Test]
        public void issue_10_should_be_able_to_fake_fields()
        {
            var faker = new Faker<Bar>()
                .RuleFor(b => b.Email, f => f.Internet.Email())
                .RuleFor(b => b.Name, f => f.Name.FirstName())
                .RuleFor(b => b.LastName, f => f.Name.LastName());

            var bar = faker.Generate();

            bar.Dump();
            bar.Name.Should().NotBeNullOrEmpty();
            bar.Email.Length.Should().BeGreaterOrEqualTo(2);
            bar.Email.Should().NotBeNullOrEmpty();
            bar.Email.Length.Should().BeGreaterOrEqualTo(2);

            bar.LastName.Should().NotBeNullOrEmpty();
            bar.LastName.Length.Should().BeGreaterOrEqualTo(2);

        }

        [Test]
        public void issue_12_bogus_should_be_thread_safe()
        {
            int threadCount = 20;

            var barId = 0;
            var faker = new Faker<Bar>()
                .RuleFor( b => b.Id, f => barId++)
                .RuleFor(b => b.Email, f => f.Internet.Email())
                .RuleFor(b => b.Name, f => f.Name.FirstName())
                .RuleFor(b => b.LastName, f => f.Name.LastName());

            var threads = new List<Task>();
            for( var x = 0; x < threadCount; x++)
            {
                var thread = Task.Run(() =>
                    {
                        var fakes = faker.Generate(3);
                        fakes.Dump();
                    });
                threads.Add(thread);
            }

            Task.WaitAll(threads.ToArray());

            Console.WriteLine(barId);
            barId.Should().Be(60);

            var result = Parallel.For(0, threadCount, i =>
                {
                    var fakes = faker.Generate(3).ToList();
                });

            Console.WriteLine(barId);
            barId.Should().Be(120);
        }

        public class ReadOnly
        {
            public string Name;

            public string NameReadOnly => Name;
        }

        [Test]
        public void issue_13_readonly_property()
        {
            var faker = new Faker<ReadOnly>()
                .StrictMode(true)
                .RuleFor(ro => ro.Name, f => f.Name.FirstName());

            faker.Validate().Should().BeTrue();
            faker.TypeProperties.Count.Should().Be(1);
        }

        [Test]
        public void issue_13_with_model()
        {
            var counter = 0;

            var faker = new Faker<TestObject>()
                .StrictMode(true)
                .RuleFor(c => c.SomeOtherId, f => counter++)
                .RuleFor(c => c.SomeId, f => Guid.NewGuid())
                .RuleFor(c => c.SomeFutureDate, f => f.Date.Future())
                .RuleFor(c => c.SomePastDate, (f, b) => b.SomeFutureDate.AddHours(f.Random.Number(1, 24)))
                .RuleFor(c => c.SomeStatusInt, (f, b) => (int)b.SomeExplicitInt)
                .RuleFor(c => c.SomeExplicitInt, f => 2)
                .RuleFor(c => c.SomeBool3, f => f.Random.Bool())
                .RuleFor(c => c.SomeBool2, f => f.Random.Bool())
                .RuleFor(c => c.SomeBool1, f => f.Random.Bool())
                .RuleFor(c => c.SomeOtherInt, f => f.Random.Number(1, 5))

                .RuleFor(c => c.SomeInt, f => 0)
                .RuleFor(c => c.SomeOtherString, f => null)
                .RuleFor(c => c.SomeOtherGuid, f => Guid.NewGuid())
                .RuleFor(c => c.SomeString, f => null)

                .RuleFor(c => c.SomeComment, f => f.Lorem.Sentence())
                .RuleFor(c => c.SomeGuid, f => null)
                .RuleFor(c => c.SomeTimestamp, f => null);

            faker.TypeProperties.Count.Should().Be(17);

            var fake = faker.Generate();
            fake.Dump();
        }

        [Test]
        public void issue_23_should_be_able_to_generate_random_word_without_exception()
        {
            var faker = new Faker<TestClass>();
            faker.RuleFor(x => x.Value, faker1 => faker1.Random.Word());
            foreach (var item in faker.Generate(1000))
            {
                item.Value.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Test]
        public void issue_49_pr_51_pickrandom_subset()
        {
            var items = Enumerable.Range(1, 10).ToArray();

            var f = new Faker();

            Action bounds1 = () =>
                {
                    f.PickRandom(items, 25).ToList();
                };

            bounds1.ShouldThrow<ArgumentOutOfRangeException>();

            Action bounds2 = () =>
                {
                    f.PickRandom(items, -1).ToList();
                };

            bounds2.ShouldThrow<ArgumentOutOfRangeException>();

            var picked = f.PickRandom(items, 4).ToArray();
            picked.Dump();
            picked.Should().Equal(2, 5, 7, 9);
        }

        public class Issue45Object
        {
            public int Id { get; set; }
            public List<string> Phones { get; set; } // PROBLEM !!!
        }

        [Test]
        public void issue_45_better_fluency()
        {
            var ids = 0;

            var test = new Faker<Issue45Object>()
                .RuleFor(p => p.Id, f => ids++)
                .RuleFor(p => p.Phones, f => f.Generate(5, () => f.Phone.PhoneNumber()).ToList());

            test.Generate(1).First().Phones.Count.Should().Be(5);
        }

        public class TestClass
        {
            public string Value { get; set; }
        }

        public class TestObject
        {
            private DateTime? _lastTimeToUnbook;

            public int SomeOtherId { get; set; }

            public Guid SomeId { get; set; }

            public DateTime SomeFutureDate { get; set; }

            public DateTime SomePastDate { get; set; }

            public int SomeStatusInt { get; set; }

            public int SomeExplicitInt
            {
                get { return this.SomeStatusInt; }
                set { this.SomeStatusInt = value; }
            }

            public bool SomeBool3 { get; set; }

            public bool SomeBool2 { get; set; }

            public bool SomeBool1 { get; set; }

            public int SomeOtherInt { get; set; }

            public DateTime? ReadOnlyDateTime
            {
                get
                {
                    return _lastTimeToUnbook;
                }
            }

            public int SomeInt { get; set; }

            public string SomeOtherString { get; set; }

            public Guid SomeOtherGuid { get; set; }

            public string SomeString { get; set; }

            public bool Someboolean
            {
                get { return !this.SomeTimestamp.HasValue; }
            }

            public DateTime? SomeTimestamp { get; set; }

            public Guid? SomeGuid { get; set; }

            public string SomeComment { get; set; }
        }
    }
}