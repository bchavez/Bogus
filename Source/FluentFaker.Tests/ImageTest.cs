using System.Net;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    [TestFixture]
    public class ImageTest : ConsistentTest
    {
        private Images image;

        [SetUp]
        public void BeforeEachTest()
        {
            image = new Images();
        }


        [Test]
        [Explicit]
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
   
    }
}