using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue193 : SeededTest
   {
      [Fact]
      public void tr_locale_should_have_real_state_name()
      {
         var f = new Faker("tr");
         
         f.Address.State().Should().Be("Kirklareli");
      }

      [Fact]
      public void can_extend_address_with_my_own_iso3166tr()
      {
         var f = new Faker("tr");

         var state = f.Address.StateIso3166();
         state.Name.Should().Be("Kirklareli");
         state.Code.Should().Be("TR-39");
      }
   }

   public static class ExtensionsForTrLocale
   {
      private static (string, string)[] Iso3166TR =
         {
            ("TR-01", "Adana"),
            ("TR-02", "Adiyaman"),
            ("TR-03", "Afyonkarahisar"),
            ("TR-04", "Agri"),
            ("TR-68", "Aksaray"),
            ("TR-05", "Amasya"),
            ("TR-06", "Ankara"),
            ("TR-07", "Antalya"),
            ("TR-75", "Ardahan"),
            ("TR-08", "Artvin"),
            ("TR-09", "Aydin"),
            ("TR-10", "Balikesir"),
            ("TR-74", "Bartin"),
            ("TR-72", "Batman"),
            ("TR-69", "Bayburt"),
            ("TR-11", "Bilecik"),
            ("TR-12", "Bingöl"),
            ("TR-13", "Bitlis"),
            ("TR-14", "Bolu"),
            ("TR-15", "Burdur"),
            ("TR-16", "Bursa"),
            ("TR-17", "Çanakkale"),
            ("TR-18", "Çankiri"),
            ("TR-19", "Çorum"),
            ("TR-20", "Denizli"),
            ("TR-21", "Diyarbakir"),
            ("TR-81", "Düzce"),
            ("TR-22", "Edirne"),
            ("TR-23", "Elazig"),
            ("TR-24", "Erzincan"),
            ("TR-25", "Erzurum"),
            ("TR-26", "Eskisehir"),
            ("TR-27", "Gaziantep"),
            ("TR-28", "Giresun"),
            ("TR-29", "Gümüshane"),
            ("TR-30", "Hakkâri"),
            ("TR-31", "Hatay"),
            ("TR-76", "Igdir"),
            ("TR-32", "Isparta"),
            ("TR-34", "Istanbul"),
            ("TR-35", "Izmir"),
            ("TR-46", "Kahramanmaras"),
            ("TR-78", "Karabük"),
            ("TR-70", "Karaman"),
            ("TR-36", "Kars"),
            ("TR-37", "Kastamonu"),
            ("TR-38", "Kayseri"),
            ("TR-71", "Kirikkale"),
            ("TR-39", "Kirklareli"),
            ("TR-40", "Kirsehir"),
            ("TR-79", "Kilis"),
            ("TR-41", "Kocaeli"),
            ("TR-42", "Konya"),
            ("TR-43", "Kütahya"),
            ("TR-44", "Malatya"),
            ("TR-45", "Manisa"),
            ("TR-47", "Mardin"),
            ("TR-33", "Mersin"),
            ("TR-48", "Mugla"),
            ("TR-49", "Mus"),
            ("TR-50", "Nevsehir"),
            ("TR-51", "Nigde"),
            ("TR-52", "Ordu"),
            ("TR-80", "Osmaniye"),
            ("TR-53", "Rize"),
            ("TR-54", "Sakarya"),
            ("TR-55", "Samsun"),
            ("TR-56", "Siirt"),
            ("TR-57", "Sinop"),
            ("TR-58", "Sivas"),
            ("TR-63", "Sanliurfa"),
            ("TR-73", "Sirnak"),
            ("TR-59", "Tekirdag"),
            ("TR-60", "Tokat"),
            ("TR-61", "Trabzon"),
            ("TR-62", "Tunceli"),
            ("TR-64", "Usak"),
            ("TR-65", "Van"),
            ("TR-77", "Yalova"),
            ("TR-66", "Yozgat"),
            ("TR-67", "Zonguldak")
         };

      public static (string Code, string Name) StateIso3166(this Address address)
      {
         return address.Random.ArrayElement(Iso3166TR);
      }
   }
}
