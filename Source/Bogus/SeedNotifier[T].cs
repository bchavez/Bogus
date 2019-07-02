using System.Collections.Generic;

namespace Bogus
{
   /// <summary>
   /// Objects should implement this interface if they use a
   /// <see cref="Randomizer"/>.
   /// </summary>
   public interface IHasRandomizer
   {
      /// <summary>
      /// Access the randomizer on the implementing object. When the property value
      /// is set, the object is instructed to use the randomizer as a source of generating
      /// random values. Additionally, setting this property also notifies any dependent
      /// via <see cref="SeedNotifier.Notify"/>. 
      /// </summary>
      Randomizer Random { set; }

      /// <summary>
      /// Retrieves the internal notifier registry for this object.
      /// </summary>
      SeedNotifier GetNotifier();
   }

   /// <summary>
   /// The seed notifier's purpose is to keep track of any objects that
   /// might need to be notified when a seed/randomizer changes.
   /// For example, the Internet dataset depends on the Name dataset 
   /// to generate data. If the randomizer seed changes in Internet, the 
   /// Name dependency data set should be notified of this change too.
   /// This whole process is important in maintaining determinism in Bogus.
   /// </summary>
   public class SeedNotifier
   {
      private List<IHasRandomizer> registry = new List<IHasRandomizer>();

      /// <summary>
      /// Causes <paramref name="item"/> to be remembered and tracked so that the
      /// <paramref name="item"/> will be notified when <see cref="Notify"/> is called.
      /// </summary>
      public U Flow<U>(U item) where U : IHasRandomizer
      {
         this.registry.Add(item);
         return item;
      }

      /// <summary>
      /// Pushes/notifies all tracked objects that a new randomizer has been set.
      /// </summary>
      public void Notify(Randomizer r)
      {
         foreach( var item in registry )
         {
            item.Random = r;
         }
      }
   }
}