using Newtonsoft.Json.Linq;

namespace FluentFaker
{
    public class Category
    {
        public Category(string locale = "en")
        {
            this.Locale = locale;
            this.CategoryName = this.GetType().Name.ToLower();
        }

        protected string CategoryName { get; set; }

        public string Locale { get; set; }

        public JToken Get(string subKind)
        {
            return Faker.Get(this.CategoryName, subKind, Locale);
        }

        public JArray GetArray(string subKind)
        {
            return (JArray)Get(subKind);
        }

        public JObject GetObject(string subKind)
        {
            return (JObject)Get(subKind);
        }

        public string GetRandomArrayItem(string subKind)
        {
            return Random.ArrayElement(GetArray(subKind));
        }
    }
}