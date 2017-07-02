using Bogus.Auto;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bogus.Tests.Auto
{
    public class AutoTests
    {
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
                context.Continuation = GenerateContinuation.Break;
            }
        }

        public interface ICalculator
        {
            decimal ItemTotal(Product product, int quantity);
        }

        public class Buyer
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Order
        {
            public Order(Guid id, Buyer buyer)
            {
                Id = id;
                Buyer = buyer;
                Items = new List<OrderItem>();
            }

            public Guid Id { get; }
            public Buyer Buyer { get; }
            public IEnumerable<OrderItem> Items { get; }
        }

        public class OrderItem
        {
            public OrderItem(Product product)
            {
                Product = product;
            }

            public Product Product { get; }
            public int Quantity { get; set; }
        }

        public class Product
        {
            public Product(string code, string name)
            {
                Code = code;
                Name = name;
            }

            public string Code { get; }
            public string Name { get; }
            public decimal? Price { get; set; }
        }

        public class ShoppingCart
        {
            public ShoppingCart(ICalculator calculator)
            {
                Calculator = calculator;
                Items = new Dictionary<Product, int>();
            }

            private ICalculator Calculator { get; }

            public IDictionary<Product, int> Items { get; }
            public decimal Total => Items.Keys.Aggregate(0m, (t, p) => t + Calculator.ItemTotal(p, Items[p]));
        }

        public AutoTests()
        {
            Generator = new AutoGenerator()
                .UsingConvention(new NSubstituteConvention())
                .RuleForType<IDictionary<Product, int>>(c => new Dictionary<Product, int>
                {
                    {Generator.Generate<Product>(), c.FakerHub.Random.Int()},
                    {Generator.Generate<Product>(), c.FakerHub.Random.Int()}
                });
        }

        private AutoGenerator Generator { get; }

        [Fact]
        public void Should_Generate_Order()
        {
            var orderFaker = new Faker<Order>();
            var orderItems = Generator.Generate<OrderItem>(3);

            Generator.RuleFor<Order, IEnumerable<OrderItem>>(b => b.Items, c => orderItems);

            var order = Generator.Generate(orderFaker);
        }

        [Fact]
        public void Should_Generate_OrderItems()
        {
            var orderItems = Generator.Generate<IEnumerable<OrderItem>>();
        }

        [Fact]
        public void Should_Generate_ShoppingCart()
        {
            var shoppingCart = Generator.Generate<ShoppingCart>();
        }




        //public class GeneratorTests
        //    : GenerationTests
        //{
        //    private IList<IConvention> _conventions;

        //    public GeneratorTests()
        //    {
        //        _conventions = new List<IConvention>();
        //        _generator = new Generator(_context.Count, _binder, _fakerHub, _conventions);
        //    }

        //    [Fact]
        //    public void Should_Throw_If_Binding_Is_Null()
        //    {
        //        Action action = () => _generator.Generate(null);

        //        action.ShouldThrow<ArgumentNullException>();
        //    }

        //    [Fact]
        //    public void Should_Call_CanInvoke_If_Conditional_Convention()
        //    {
        //        var convention = Substitute.For<IConditionalConvention>();
        //        _conventions.Add(convention);

        //        _generator.Generate(_binding);

        //        convention.Received().CanInvoke(_binding);
        //    }

        //    [Fact]
        //    public void Should_Call_Invoke_If_Not_Conditional_Convention()
        //    {
        //        var convention = Substitute.For<IConvention>();
        //        _conventions.Add(convention);

        //        _generator.Generate(_binding);

        //        convention.Received().Invoke(Arg.Any<GenerateContext>());
        //    }

        //    [Fact]
        //    public void Should_Call_Invoke_If_Conditional_Convention_And_True()
        //    {
        //        var convention = Substitute.For<IConditionalConvention>();
        //        convention.CanInvoke(_binding).Returns(true);

        //        _conventions.Add(convention);

        //        _generator.Generate(_binding);

        //        convention.Received().Invoke(Arg.Any<GenerateContext>());
        //    }

        //    [Fact]
        //    public void Should_Not_Call_Invoke_If_Conditional_Convention_And_False()
        //    {
        //        var convention = Substitute.For<IConditionalConvention>();
        //        convention.CanInvoke(_binding).Returns(false);

        //        _conventions.Add(convention);

        //        _generator.Generate(_binding);

        //        convention.DidNotReceive().Invoke(Arg.Any<GenerateContext>());
        //    }

        //    [Fact]
        //    public void Should_Call_Invoke_With_GenerateContext()
        //    {
        //        var count = _context.Count;
        //        var convention = Substitute.For<IConvention>();

        //        _context = null;
        //        _conventions.Add(convention);

        //        convention
        //            .When(x => x.Invoke(Arg.Any<GenerateContext>()))
        //            .Do(x => _context = x.Arg<GenerateContext>());

        //        _generator.Generate(_binding);

        //        _context.ShouldBeEquivalentTo(new
        //        {
        //            Count = count,
        //            Generator = _generator,
        //            Binder = _binder,
        //            Binding = _binding,
        //            FakerHub = _fakerHub
        //        });
        //    }
        //}

    }
}
