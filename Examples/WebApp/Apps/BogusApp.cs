using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Immutable;
using Ivy.Shared;
using Ivy.Views.Builders;
using Ivy.Views.Forms;
using Bogus;
using Bogus.DataSets;

namespace BogusExample.Apps
{
    [App(icon: Icons.BrainCog)]
    public class BogusApp : ViewBase
    {
        public override object? Build()
        {
            var orders = this.UseState(ImmutableArray.Create<Order>());
            var fruits = this.UseState(ImmutableArray.Create("apple", "banana", "orange", "strawberry", "kiwi"));
            var fruitInput = this.UseState("");

            // Layout constants
            const float LeftCardWidth = 0.45f;
            const float RightCardWidth = 0.55f;

            // Left Card: Fruit Configuration
            var leftCardBody = Layout.Vertical().Gap(4).Padding(3)
              | Text.H2("Fruit Configuration")
              | Text.Muted("Manage the list of fruits for orders")
              // Add fruit input
              | (Layout.Horizontal().Width(Size.Full()).Gap(2)
                 | fruitInput.ToTextInput(placeholder: "Add a new fruit...").Width(Size.Grow())
                 | new Button("Add", _ =>
                   {
                       var f = (fruitInput.Value ?? "").Trim();
                       if (f.Length == 0) return;

                       if (!fruits.Value.Any(x => x.Equals(f, StringComparison.OrdinalIgnoreCase)))
                           fruits.Set(fruits.Value.Add(f));

                       fruitInput.Set("");
                   }).Icon(Icons.Plus).Variant(ButtonVariant.Secondary)
              )

              // Fruits list in Expandable
              | new Expandable(
                  $"Available Fruits ({fruits.Value.Length})",
                  fruits.Value.Length > 0
                      ? Layout.Vertical().Gap(2)
                        | fruits.Value.Select(f =>
                              Layout.Horizontal().Width(Size.Full()).Gap(2).Align(Align.Center)
                              | Text.Literal(f).Width(Size.Grow())
                              | new Button(null, _ => fruits.Set(fruits.Value.Remove(f)))
                                  .Icon(Icons.Trash)
                                  .Variant(ButtonVariant.Ghost)
                          )
                      : (object)Text.Muted("No fruits added yet. Add some fruits to generate orders.")
              )

              // Generate button
              | (Layout.Horizontal().Width(Size.Full()).Gap(2)
                 | new Button("Generate 10 Orders", _ =>
                   {
                       var fruit = (fruits.Value.Length > 0
                           ? fruits.Value
                           : ImmutableArray.Create("apple", "banana", "orange", "strawberry", "kiwi"))
                           .ToArray();

                       var orderIds = 0;

                       var testOrders = new Faker<Order>()
                           .StrictMode(true)
                           .RuleFor(o => o.OrderId, f => orderIds++)
                           .RuleFor(o => o.Item, f => f.PickRandom(fruit))
                           .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
                           .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));

                       var generated = testOrders.Generate(10);
                       orders.Set(ImmutableArray.CreateRange(generated));
                   })
                   .Icon(Icons.RefreshCw)
                   .Variant(ButtonVariant.Primary)
                   .Width(Size.Full())

                 | new Button("Clear", _ => orders.Set(ImmutableArray.Create<Order>()))
                   .Icon(Icons.Trash2)
                   .Variant(ButtonVariant.Outline)
              )
              | new Spacer()
              | Text.Small("This demo uses Bogus library to generate fake data.")
              | Text.Markdown("Built with [Ivy Framework](https://github.com/Ivy-Interactive/Ivy-Framework) and [Bogus](https://github.com/bchavez/Bogus)");

            var leftCard = new Card(leftCardBody).Width(Size.Fraction(LeftCardWidth)).Height(Size.Fit().Min(Size.Full()));
            
            // Right Card: Generated Orders Table
            var rightCardBody = Layout.Vertical().Gap(4).Padding(3)
              | Text.H2("Generated Orders")
              | Text.Muted($"Displaying {orders.Value.Length} orders")
              | orders.Value.ToTable()
                  .Width(Size.Full())
                  .Width(p => p.OrderId, Size.Fraction(0.15f))
                  .Width(p => p.Item, Size.Fraction(0.35f))
                  .Width(p => p.Quantity, Size.Fraction(0.2f))
                  .Width(p => p.LotNumber, Size.Fraction(0.3f))
                  .Header(p => p.OrderId, "ID")
                  .Header(p => p.Item, "Fruit")
                  .Header(p => p.Quantity, "Qty")
                  .Header(p => p.LotNumber, "Lot #")
                  .Align(p => p.OrderId, Align.Center)
                  .Align(p => p.Quantity, Align.Center)
                  .Align(p => p.LotNumber, Align.Center)
                  .Empty(
                      Layout.Vertical().Gap(3)
                      | Text.Muted("No orders yet.")
                      | Text.Muted("Generate orders using the left panel.")
                  );

            var rightCard = new Card(rightCardBody).Width(Size.Fraction(RightCardWidth)).Height(Size.Fit().Min(Size.Full()));

            return Layout.Horizontal().Gap(6)
                  | leftCard
                  | rightCard;
        }

        internal class Order
        {
            public int OrderId { get; set; }
            public string Item { get; set; }
            public int Quantity { get; set; }
            public int? LotNumber { get; set; }
        }
    }
}
