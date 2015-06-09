using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentFaker
{
    public static class PropertyName
    {
        public static string For<T,TProp>(Expression<Func<T, TProp>> expression)
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
            MemberExpression memberExpression;

            var unary = expression as UnaryExpression;
            if( unary != null )
                //In this case the return type of the property was not object,
                //so .Net wrapped the expression inside of a unary Convert()
                //expression that casts it to type object. In this case, the
                //Operand of the Convert expression has the original expression.
                memberExpression = unary.Operand as MemberExpression;
            else
            //when the property is of type object the body itself is the
            //correct expression
                memberExpression = expression as MemberExpression;

            if( memberExpression == null
                || !( memberExpression.Member is PropertyInfo ) )
                throw new ArgumentException(
                    "Expression was not of the form 'x =&gt; x.property'.");

            return memberExpression.Member.Name;
        }
    }
}