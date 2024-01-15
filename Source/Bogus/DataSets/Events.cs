using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bogus.DataSets
{
   public class Events : DataSet
   {
      public string History()
      {
         return this.GetRandomArrayItem("history");
      }
   }
}