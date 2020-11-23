using System;

public class NoParameterlessCtorClass
{
	public NoParameterlessCtorClass(object obj, object obj2)
	{
      Obj = obj;
      Obj2 = obj2;
   }

   public object Obj { get; }
   public object Obj2 { get; set; }
   public object Obj3;
}
