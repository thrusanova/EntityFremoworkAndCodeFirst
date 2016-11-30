
namespace ProductsShop.Client
{
    using Models;
    using Newtonsoft.Json;
    using Data;
    using System.Collections.Generic;
    using System.IO;
    using System;
    using System.Linq;
    class Program
    {
        private const string usersPath = "../../../datasets/users.json";
        private const string productsPath = "../../../datasets/products.json";
        private const string categoriesPath = "../../../datasets/categories.json";

      
        static void Main(string[] args)
        {
            ImportUsers();
            ImportProducts();
            ImportCategories();
        }
        private static void ImportUsers()
        {
            var context = new ProductsShopContext();
            var json = File.ReadAllText(usersPath);
            var userEntity = JsonConvert.DeserializeObject<IEnumerable<User>>(json);
            foreach (var user in userEntity)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

        }

        private static void ImportCategories()
        {
            var context = new ProductsShopContext();
            var json = File.ReadAllText(categoriesPath);
            var categoryEntity = JsonConvert.DeserializeObject<IEnumerable<Category>>(json);
            foreach (var category in categoryEntity)
            {
                context.Categories.Add(category);
            }
            context.SaveChanges();
        }

        private static void ImportProducts()
        {
            var context = new ProductsShopContext();
            var json = File.ReadAllText(productsPath);
            var productEntity = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
            var rand = new Random();
            foreach (var product in productEntity)
            {
                double shouldHaveBuyer = rand.NextDouble();
                product.SellerId = rand.Next(1, context.Users.Count() + 1);
                if (shouldHaveBuyer<=0.7)
                {
                    product.BuyerId = rand.Next(1, context.Users.Count() + 1);
                }
                context.Products.Add(product);
            }
            context.SaveChanges();
            
        }
    }
}
