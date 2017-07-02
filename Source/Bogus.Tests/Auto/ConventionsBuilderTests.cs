using Bogus.Auto;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bogus.Tests.Auto
{
    public class ConventionsBuilderTests
    {
        private IConvention _convention;
        private ConventionsBuilder _builder;

        public ConventionsBuilderTests()
        {
            _convention = Substitute.For<IConvention>();
            _builder = new ConventionsBuilder();
        }

        public class AddTests
            : ConventionsBuilderTests
        {
            [Fact]
            public void Should_Throw_If_Convention_Is_Null()
            {
                Action action = () => _builder.Add(null);

                action.ShouldThrowExactly<ArgumentNullException>();
            }

            [Fact]
            public void Should_Add_Convention_To_Pipeline()
            {
                _builder.Add(_convention);

                _builder.Conventions.Pipeline.Should().OnlyContain(p => p.Convention == _convention);
            }

            [Fact]
            public void Should_Return_Builder()
            {
                _builder.Add(_convention).Should().Be(_builder);
            }
        }

        public class SkipTests
            : ConventionsBuilderTests
        {

        }

        public class ClearTests
            : ConventionsBuilderTests
        {
            [Fact]
            public void Should_Clear_Pipeline_Items()
            {
                var conventions = new Conventions()
                {
                    Pipeline = new List<ConventionPipelineItem>
                    {
                        new ConventionPipelineItem(_convention, ConventionPipeline.Default)
                    }
                };

                conventions.Clear();

                conventions.Pipeline.Should().BeEmpty();
            }
        }

        public class BuildTests
            : ConventionsBuilderTests
        {

        }
    }
}
