namespace Bogus
{
  internal sealed class AutoFakerContext
  {
    internal AutoFakerContext(string locale, IAutoBinder binder)
    {
      this.Locale = locale ?? "en";
      this.Binder = binder ?? new AutoBinder();
    }
    
    internal string Locale { get; }
    internal IAutoBinder Binder { get; }
  }
}
