using Bogus.Auto;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bogus.Tests.Auto
{
    public class GenerationTests
    {
        private IBinder _binder;
        private Faker _fakerHub;
        private BindingInfo _binding;
        private IGenerator _generator;
        private GenerateContext _context;

        public GenerationTests()
        {
            _binder = Substitute.For<IBinder>();
            _fakerHub = new Faker();
            _binding = new BindingInfo(typeof(object), "NAME");
            _generator = Substitute.For<IGenerator>();
            _context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);
        }

        public class GeneratorTests
            : GenerationTests
        {
            private IList<IConvention> _conventions;

            public GeneratorTests()
            {
                _conventions = new List<IConvention>();
                _generator = new Generator(_context.Count, _binder, _fakerHub, _conventions);
            }

            [Fact]
            public void Should_Throw_If_Binding_Is_Null()
            {
                Action action = () => _generator.Generate(null);

                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Should_Call_CanInvoke_If_Conditional_Convention()
            {
                var convention = Substitute.For<IConditionalConvention>();
                _conventions.Add(convention);

                _generator.Generate(_binding);

                convention.Received().CanInvoke(_binding);
            }

            [Fact]
            public void Should_Call_Invoke_If_Not_Conditional_Convention()
            {
                var convention = Substitute.For<IConvention>();
                _conventions.Add(convention);

                _generator.Generate(_binding);

                convention.Received().Invoke(Arg.Any<GenerateContext>());
            }

            [Fact]
            public void Should_Call_Invoke_If_Conditional_Convention_And_True()
            {
                var convention = Substitute.For<IConditionalConvention>();
                convention.CanInvoke(_binding).Returns(true);

                _conventions.Add(convention);

                _generator.Generate(_binding);

                convention.Received().Invoke(Arg.Any<GenerateContext>());
            }

            [Fact]
            public void Should_Not_Call_Invoke_If_Conditional_Convention_And_False()
            {
                var convention = Substitute.For<IConditionalConvention>();
                convention.CanInvoke(_binding).Returns(false);

                _conventions.Add(convention);

                _generator.Generate(_binding);

                convention.DidNotReceive().Invoke(Arg.Any<GenerateContext>());
            }

            [Fact]
            public void Should_Call_Invoke_With_GenerateContext()
            {
                var count = _context.Count;
                var convention = Substitute.For<IConvention>();

                _context = null;
                _conventions.Add(convention);

                convention
                    .When(x => x.Invoke(Arg.Any<GenerateContext>()))
                    .Do(x => _context = x.Arg<GenerateContext>());

                _generator.Generate(_binding);

                _context.ShouldBeEquivalentTo(new
                {
                    Count = count,
                    Generator = _generator,
                    Binder = _binder,
                    Binding = _binding,
                    FakerHub = _fakerHub
                });
            }
        }

        public class GenerateTests
            : GenerationTests
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                Action action = () =>
                {
                    _context = null;
                    _context.Generate(_binding);
                };

                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Should_Throw_If_Binding_Is_Null()
            {
                Action action = () => _context.Generate(null);

                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Should_Call_Generator_Generate()
            {
                _context.Generate(_binding);

                _generator.Received().Generate(Arg.Any<BindingInfo>());
            }

            [Fact]
            public void Should_Create_A_New_BindingInfo()
            {
                BindingInfo binding = null;

                _generator
                    .When(x => x.Generate(Arg.Any<BindingInfo>()))
                    .Do(x => binding = x.Arg<BindingInfo>());

                _context.Generate(_binding);

                binding.Should().NotBe(_binding).And.Subject.ShouldBeEquivalentTo(_binding);
            }

            [Fact]
            public void Should_Return_Generated_Binding_Value()
            {
                var value = new object();

                _generator
                    .When(x => x.Generate(Arg.Any<BindingInfo>()))
                    .Do(x => x.Arg<BindingInfo>().Value = value);

                _context.Generate(_binding).Should().Be(value);
            }
        }

        public class GenerateManyTests
            : GenerationTests
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                Action action = () =>
                {
                    _context = null;
                    _context.GenerateMany(_binding);
                };

                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Should_Throw_If_Binding_Is_Null()
            {
                Action action = () => _context.GenerateMany(null);

                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Should_Call_Generator_Generate()
            {
                _context.GenerateMany(_binding);

                _generator.Received().Generate(Arg.Any<BindingInfo>());
            }

            [Fact]
            public void Should_Create_A_New_BindingInfo()
            {
                BindingInfo binding = null;

                _generator
                    .When(x => x.Generate(Arg.Any<BindingInfo>()))
                    .Do(x => binding = x.Arg<BindingInfo>());

                _context.GenerateMany(_binding);

                binding.Should().NotBe(_binding).And.Subject.ShouldBeEquivalentTo(_binding);
            }

            [Fact]
            public void Should_Return_Generated_Binding_Values()
            {
                var value = new object();

                _generator
                    .When(x => x.Generate(Arg.Any<BindingInfo>()))
                    .Do(x => x.Arg<BindingInfo>().Value = value);

                _context.GenerateMany(_binding).Should().Contain(new[] { value });
            }

            [Fact]
            public void Should_Throw_If_Context_Is_Null_And_Has_Count()
            {
                Action action = () =>
                {
                    var count = (int)DateTime.Now.DayOfWeek;

                    _context = null;
                    _context.GenerateMany(count, _binding);
                };

                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void Should_Return_Generated_Binding_Values_For_Count()
            {
                var values = new List<object>();
                var count = (int)DateTime.Now.DayOfWeek;

                _generator
                    .When(x => x.Generate(Arg.Any<BindingInfo>()))
                    .Do(x => 
                    {
                        var value = new object();
                        x.Arg<BindingInfo>().Value = value;

                        values.Add(value);
                    });

                _context.GenerateMany(count, _binding).Should().Contain(values);
            }
        }
    }
}
