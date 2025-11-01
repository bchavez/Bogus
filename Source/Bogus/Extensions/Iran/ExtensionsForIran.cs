namespace Bogus.Extensions.Iran;

/// <summary>
/// API extensions specific for Iran.
/// </summary>
public static class ExtensionsForIran
{
   /// <summary>
   /// Generates a fake Iranian National Number (کد ملی) with a valid checksum.
   /// </summary>
   /// <param name="p">The person for whom to generate the national number.</param>
   /// <returns>A valid 10-digit Iranian national number as a string.</returns>
   public static string IranianNationalNumber(this Person p)
   {
      const string Key = nameof(ExtensionsForIran) + nameof(IranianNationalNumber);
      if (p.context.TryGetValue(Key, out var value))
      {
         return (string)value;
      }

      Randomizer randomizer = p.Random;

      int[] list = new int[10];
      int sum = 0;

      for (int i = 0; i < 9; i++)
      {
         list[i] = randomizer.Number(9);
         sum += list[i] * (10 - i);
      }

      int s = sum % 11;
      list[9] = s < 2 ? s : 11 - s;

      string result = string.Concat(list);
      p.context[Key] = result;

      return result;
   }
}
