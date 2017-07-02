using Bogus.Auto;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bogus.Tests.Auto
{
    public class GenerateContextTests
    {
        private IBinder _binder;
        private Faker _fakerHub;
        private BindingInfo _binding;
        private IGenerator _generator;
        private GenerateContext _context;

        public GenerateContextTests()
        {
            _binder = Substitute.For<IBinder>();
            _fakerHub = new Faker();
            _binding = new BindingInfo(typeof(object), "NAME");
            _generator = Substitute.For<IGenerator>();
            _context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);
        }

        public class GenerateTests
            : GenerateContextTests
        {
            [Fact]
            public void Should_Call_Generator_Generate()
            {
                _context.Generate(_binding);

                _generator.Received().Generate(Arg.Any<GenerateContext>());
            }

            [Fact]
            public void Should_Create_A_New_BindingInfo()
            {
                GenerateContext context = null;

                _generator
                    .When(x => x.Generate(Arg.Any<GenerateContext>()))
                    .Do(x => context = x.Arg<GenerateContext>());

                _context.Generate(_binding);

                context.Binding.Should().NotBe(_binding).And.Subject.ShouldBeEquivalentTo(_binding);
            }

            [Fact]
            public void Should_Return_Generated_Binding_Value()
            {
                var value = new object();

                _generator
                    .When(x => x.Generate(Arg.Any<GenerateContext>()))
                    .Do(x => x.Arg<GenerateContext>().Binding.Value = value);

                _context.Generate(_binding).Should().Be(value);
            }
        }

        public class GenerateManyTests
            : GenerateContextTests
        {
            [Fact]
            public void Should_Call_Generator_Generate()
            {
                _context.GenerateMany(_binding);

                _generator.Received().Generate(Arg.Any<GenerateContext>());
            }

            [Fact]
            public void Should_Create_A_New_BindingInfo()
            {
                GenerateContext context = null;

                _generator
                    .When(x => x.Generate(Arg.Any<GenerateContext>()))
                    .Do(x => context = x.Arg<GenerateContext>());

                _context.GenerateMany(_binding);

                context.Binding.Should().NotBe(_binding).And.Subject.ShouldBeEquivalentTo(_binding);
            }

            [Fact]
            public void Should_Return_Generated_Binding_Values()
            {
                var value = new object();

                _generator
                    .When(x => x.Generate(Arg.Any<GenerateContext>()))
                    .Do(x => x.Arg<GenerateContext>().Binding.Value = value);

                _context.GenerateMany(_binding).Should().Contain(new[] { value });
            }

            [Fact]
            public void Should_Return_Generated_Binding_Values_For_Count()
            {
                var values = new List<object>();
                var count = (int)DateTime.Now.DayOfWeek;

                _generator
                    .When(x => x.Generate(Arg.Any<GenerateContext>()))
                    .Do(x => 
                    {
                        var value = new object();
                        x.Arg<GenerateContext>().Binding.Value = value;

                        values.Add(value);
                    });

                _context.GenerateMany(count, _binding).Should().Contain(values);
            }
        }
    }
}
