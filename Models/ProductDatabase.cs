using System.Collections.Generic;
using System.Linq;

namespace Reviews.Models
{
    public static class ProductDatabase
    {
        static ProductDatabase()
        {
            // Pre-fill data.
            AddProduct("This is a book")
             .AddReview("I like it!", "Arya Stark");

            AddProduct("This is a MacBook")
             .AddReview("Too expensive!", "Sansa Stark");

            AddProduct("This is a BookBook")
             .AddReview("Swedish style!", "Jon Snow");
        }

        private static List<Product> products = new List<Product>();

        public static Product AddProduct(string name)
        {
            var product = new Product(name);
            products.Add(product);
            return product;
        }

        public static Product FindProduct(string id)
        {
            return products.FirstOrDefault(product => product.Id == id);
        }

        public static IEnumerable<Product> AllProducts()
        {
            return products;
        }
    }
}