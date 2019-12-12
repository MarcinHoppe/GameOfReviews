using System.Collections.Generic;
using System.Linq;

namespace Reviews.Models
{
    public static class ProductDatabase
    {
        static ProductDatabase()
        {
            // Pre-fill data.
            AddProduct("This is a book", "Sansa Stark")
             .AddReview("I like it!", "Arya Stark");

            AddProduct("This is a MacBook", "Sansa Stark")
             .AddReview("Too expensive!", "Sansa Stark");

            AddProduct("This is a BookBook", "Sansa Stark")
             .AddReview("Swedish style!", "Jon Snow");
        }

        private static List<Product> products = new List<Product>();

        public static Product AddProduct(string name, string owner)
        {
            var product = new Product(name, owner);
            products.Add(product);
            return product;
        }

        public static Product FindProduct(string id)
        {
            return products.FirstOrDefault(product => product.Id == id);
        }

        public static void RemoveProduct(string id)
        {
            products.Remove(FindProduct(id));
        }

        public static IEnumerable<Product> AllProducts()
        {
            return products;
        }
    }
}