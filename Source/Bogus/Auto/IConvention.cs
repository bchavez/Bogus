namespace Bogus.Auto
{
    public interface IConvention
    {
        void Invoke(GenerateContext context);
    }
}
