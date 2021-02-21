using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Bogus
{
   [EditorBrowsable(EditorBrowsableState.Never)]
   public static class PropertyName
   {
      public static string For<T, TProp>(Expression<Func<T, TProp>> expression)
      {
         Expression body = expression.Body;
         return GetMemberName(body);
      }

      public static string For<T>(Expression<Func<T, object>> expression)
      {
         Expression body = expression.Body;
         return GetMemberName(body);
      }

      public static string For(Expression<Func<object>> expression)
      {
         Expression body = expression.Body;
         return GetMemberName(body);
      }

      public static string GetMemberName(Expression expression)
      {
         var expressionString = expression.ToString();
         if( expressionString.IndexOf('.') != expressionString.LastIndexOf('.') )
         {
            throw new ArgumentException(
               $"Your expression '{expressionString}' cant be used. Nested accessors like 'o => o.NestedObject.Foo' at " +
               $"a parent level are not allowed. You should create a dedicated faker for " +
               $"NestedObject like new Faker<NestedObject>().RuleFor(o => o.Foo, ...) with its own rules " +
               $"that define how 'Foo' is generated. " +
               "See this GitHub issue for more info: https://github.com/bchavez/Bogus/issues/115");
         }

         MemberExpression memberExpression;

         if( expression is UnaryExpression unary )
            //In this case the return type of the property was not object,
            //so .Net wrapped the expression inside of a unary Convert()
            //expression that casts it to type object. In this case, the
            //Operand of the Convert expression has the original expression.
            memberExpression = unary.Operand as MemberExpression;
         else
            //when the property is of type object the body itself is the
            //correct expression
            memberExpression = expression as MemberExpression;

         if( memberExpression == null )
            throw new ArgumentException(
               "Expression was not of the form 'x => x.Property or x => x.Field'.");

         return memberExpression.Member.Name;
      }
   }
}