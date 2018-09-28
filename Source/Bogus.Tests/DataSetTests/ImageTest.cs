using System.IO;
using System.Net;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.DataSetTests
{
   public class ImageTest : SeededTest
   {
      private readonly ITestOutputHelper console;

      public ImageTest(ITestOutputHelper console)
      {
         this.console = console;
         image = new Images();
      }

      private readonly Images image;

      [Fact(Skip = "Explicit")]
      public void DownloadAllTest()
      {
         var wc = new WebClient();
         wc.DownloadFile(image.Abstract(), "abstract.jpg");
         wc.DownloadFile(image.Animals(), "animals.jpg");
         wc.DownloadFile(image.Business(), "business.jpg");
         wc.DownloadFile(image.Cats(), "cats.jpg");
         wc.DownloadFile(image.City(), "city.jpg");
         wc.DownloadFile(image.Food(), "food.jpg");
         wc.DownloadFile(image.Nightlife(), "nightlife.jpg");
         wc.DownloadFile(image.Fashion(), "fashion.jpg");
         wc.DownloadFile(image.People(), "people.jpg");
         wc.DownloadFile(image.Nature(), "nature.jpg");
         wc.DownloadFile(image.Sports(), "sports.jpg");
         wc.DownloadFile(image.Technics(), "technics.jpg");
         wc.DownloadFile(image.Transport(), "transport.jpg");
      }

      [Fact]
      public void svg_data_url()
      {
         var html = @"
<html>
   <head></head>
   <body>
      <h1>ffff</h1>
      <img src='{imgdata}' />
      <h2>dddd</h2>
   </body>
</html>
";

         var dataUri = image.DataUri(200, 300, "red");

         console.Dump(dataUri);
         var content = html.Replace("{imgdata}", dataUri);
         var filename = Path.ChangeExtension(Path.GetRandomFileName(), "html");
         var file = Path.Combine(Path.GetTempPath(), filename);
         File.WriteAllText(file, content);
         console.Dump(file);

         image.DataUri(200, 300).Should()
            .Be(
               "data:image/svg+xml;charset=UTF-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20version%3D%221.1%22%20baseProfile%3D%22full%22%20width%3D%22200%22%20height%3D%22300%22%3E%3Crect%20width%3D%22100%25%22%20height%3D%22100%25%22%20fill%3D%22grey%22%2F%3E%3Ctext%20x%3D%22100%22%20y%3D%22150%22%20font-size%3D%2220%22%20alignment-baseline%3D%22middle%22%20text-anchor%3D%22middle%22%20fill%3D%22white%22%3E200x300%3C%2Ftext%3E%3C%2Fsvg%3E");
      }

      [Fact]
      public void url_generated_should_have_https()
      {
         image.Sports(https: true).Should().StartWith("https://");
      }
   }
}