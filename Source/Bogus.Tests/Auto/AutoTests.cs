using Bogus.Auto;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bogus.Tests.Auto
{
    public class AutoTests
    {
        public interface IPayrollCalculator
        {
            decimal Calculate();
        }

        public class Company
        {
            public Company(Guid id, string name, IPayrollCalculator calculator)
            {
                Id = id;
                Name = name;
                Calculator = calculator;
            }

            public Guid Id { get; }
            public string Name { get; }
            public IPayrollCalculator Calculator { get; }
            public IEnumerable<PersonTest.User> Employees { get; set; }
        }

        private class NSubstituteConvention
            : GenericReferenceTypeConvention
        {
            protected override bool CanInvoke<TType>(BindingInfo binding)
            {
                return binding.Value == null && 
                      (binding.Type.IsInterface || binding.Type.IsAbstract);
            }

            protected override void Invoke<TType>(GenerateContext context)
            {
                context.Binding.Value = Substitute.For<TType>();
            }
        }

        [Fact]
        public void Should_Auto_Generate()
        {
            // Add a global convention to mock auto generated values
            GlobalConventions.Conventions.Clear();
            GlobalConventions.Conventions.Add(new NSubstituteConvention(), ConventionPipeline.End);

            // Create and auto generate a company instance
            var faker = new Faker<Company>();

            var company1 = faker.AutoGenerate(options =>
            {
                options.Add("FirstName", b => b.Name == "FirstName", c => c.Binding.Value = "TestFirstName");
            });

            var company2 = faker.AutoGenerate(options =>
            {
                options.Skip(ConventionGroup.NameBinding);
            });
        }
    }
}
