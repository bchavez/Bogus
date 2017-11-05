using System.Net;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class ImageTest : SeededTest
   {
      public ImageTest()
      {
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
         image.DataUri(200, 300).Dump();

         image.DataUri(200, 300).Should()
            .Be(
               "data:image/svg+xml;charset=UTF-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20version%3D%221.1%22%20baseProfile%3D%22full%22%20width%3D%22200%22%20height%3D%22300%22%3E%20%3Crect%20width%3D%22100%25%22%20height%3D%22100%25%22%20fill%3D%22grey%22%2F%3E%20%20%3Ctext%20x%3D%220%22%20y%3D%2220%22%20font-size%3D%2220%22%20text-anchor%3D%22start%22%20fill%3D%22white%22%3E200x300%3C%2Ftext%3E%20%3C%2Fsvg%3E");
      }

      [Fact]
      public void url_generated_should_have_https()
      {
         image.Sports(https: true).Should().StartWith("https://");
      }
   }
}