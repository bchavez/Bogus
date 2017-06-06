namespace Bogus
{
  internal interface IAutoGenerator
  {
    object Generate(AutoGenerateContext context);
  }
}
