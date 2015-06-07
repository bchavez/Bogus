using System;

namespace FluentFaker
{
    public class Images : Category
    {
        public Images(string locale = "en") : base(locale)
        {
            this.CategoryName = "image";
        }

        public string Image()
        {
            var categories = new[]
                {"abstract", "animals", "business", "cats", "city", "food", "nightlife", "fashion", "people", "nature", "sports", "technics", "transport"};

            var picked = Random.ArrayElement(categories);
            return ImageUrl(picked);
        }

        // we can do some extra work here
        // lorempixel supports greyscale and image index
        public string ImageUrl( string category, int width = 640, int height = 480)
        {
            var path = string.Format("http://lorempixel.com/{0}/{1}/{2}", width, height, category);
            return string.IsNullOrWhiteSpace(category) ? path.Trim('/') : path;
        }

        public string Abstract(int width = 640, int height = 480)
        {
            return ImageUrl("abstract", width, height);
        }
        public string Animals(int width = 640, int height = 480)
        {
            return ImageUrl("animals", width, height);
        }
        public string Business(int width = 640, int height = 480)
        {
            return ImageUrl("business", width, height);
        }
        public string Cats(int width = 640, int height = 480)
        {
            return ImageUrl("cats", width, height);
        }
        public string City(int width = 640, int height = 480)
        {
            return ImageUrl("city", width, height);
        }

        public string Food(int width = 640, int height = 480)
        {
            return ImageUrl("food", width, height);

        }
        public string Nightlife(int width = 640, int height = 480)
        {
            return ImageUrl("nightlife", width, height);

        }
        public string Fashion(int width = 640, int height = 480)
        {
            return ImageUrl("fashion", width, height);

        }
        public string People(int width = 640, int height = 480)
        {
            return ImageUrl("people", width, height);

        }
        public string Nature(int width = 640, int height = 480)
        {
            return ImageUrl("nature", width, height);

        }

        public string Sports(int width = 640, int height = 480)
        {
            return ImageUrl("sports", width, height);

        }
        public string Technics(int width = 640, int height = 480)
        {
            return ImageUrl("technics", width, height);

        }
        public string Transport(int width = 640, int height = 480)
        {
            return ImageUrl("transport", width, height);

        }
    }
}