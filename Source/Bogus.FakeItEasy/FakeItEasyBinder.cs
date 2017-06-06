using System.Reflection;
using FakeItEasy;

namespace Bogus.FakeItEasy
{
   /// <summary>
   /// A class that enables FakeItEasy binding for interface and abstract types.
   /// </summary>
   public sealed class FakeItEasyBinder : AutoBinder
   {
      /// <summary>
      /// Creates an instance of <typeparamref name="TType"/>.
      /// </summary>
      /// <typeparam name="TType">The type of instance to create.</typeparam>
      /// <param name="context">The <see cref="AutoGenerateContext"/> instance for the generate request.</param>
      /// <returns>The created instance of <typeparamref name="TType"/>.</returns>
      public override TType CreateInstance<TType>(AutoGenerateContext context)
      {
         var type = typeof(TType);
#if STANDARD
         var typeInfo = type.GetTypeInfo();
#else
      var typeInfo = type;
#endif

         if (typeInfo.IsInterface || typeInfo.IsAbstract)
         {
            return A.Fake<TType>();
         }

         return base.CreateInstance<TType>(context);
      }
   }
}
