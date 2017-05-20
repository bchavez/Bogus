﻿using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
    public class HackerTests : SeededTest
    {
        public HackerTests()
        {
            hacker = new Hacker();
        }

        private readonly Hacker hacker;

        [Fact]
        public void can_get_a_hacker_phrase()
        {
            hacker.Phrase().Should().Be("Use the neural RAM driver, then you can calculate the neural driver!");
        }

        [Fact]
        public void make_sure_we_have_updated_ru_hacker_locale()
        {
            var ruhacker = new Hacker("ru");
            ruhacker.Adjective().Should().Be("многобайтный");
            ruhacker.Noun().Should().Be("пропускная способность");
            ruhacker.Verb().Should().Be("передавать");
            ruhacker.IngVerb().Should().Be("определение количества");
        }
    }
}