namespace Bogus.Auto
{
    public interface IConditionalConvention
        : IConvention
    {
        bool CanInvoke(BindingInfo binding);
    }
}
