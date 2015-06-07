using Newtonsoft.Json.Linq;

namespace FluentFaker
{
    public class Category
    {
        public Category(string locale = "en")
        {
            Locale = locale;
        }

        public string Locale { get; set; }

        public JToken Get(string subKind)
        {
            return Faker.Get(this.GetType().Name.ToLower(), subKind, Locale);
        }

        public JArray GetArray(string subKind)
        {
            return (JArray)Get(subKind);
        }

        public string GetArrayItem(string subKind)
        {
            return Random.ArrayElement(GetArray(subKind));
        }
    }
}