using FluentAssertions;
using Bogus.Generators;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class CardTests : SeededTest
    {
        [Test]
        public void should_be_able_to_get_a_contextually_bogus_person()
        {
            var card = new Person();

            var json = @"{
  'Name': 'Lee',
  'UserName': 'Lee_Brown3',
  'Avatar': 'https://s3.amazonaws.com/uifaces/faces/twitter/yayteejay/128.jpg',
  'Email': 'Lee_Brown3.Bechtelar30@yahoo.com',
  'DateOfBirth': '1963-06-22T09:46:17.464211',
  'Address': {
    'Street': '93255 Emerson Court',
    'Suite': 'Suite 819',
    'City': 'Fay furt',
    'ZipCode': '98784',
    'Geo': {
      'Lat': -41.0153,
      'Lng': 21.752299999999991
    }
  },
  'Phone': '(079) 088-3650',
  'Website': 'tyreek.biz',
  'Company': {
    'Name': 'Kassulke Inc',
    'CatchPhrase': 'Distributed disintermediate array',
    'Bs': 'B2B target eyeballs'
  }
}
";
            var truth = JsonConvert.DeserializeObject<Person>(json);

            card.ShouldBeEquivalentTo(truth);
        }
    }
}
