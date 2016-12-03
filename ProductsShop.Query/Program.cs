
namespace ProductsShop.Query
{
    using System;
    using Data;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections;
    using Models;
    using System.IO;
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ProductsShopContext();
             ExportProductsInRange(context);
             ExportProductsCategory(context);
             ExportSoldProducts(context);
             ExportUsersAndProducts(context);

        }

        private static void ExportUsersAndProducts(ProductsShopContext context)
        {
            throw new NotImplementedException();
        }

        private static void ExportSoldProducts(ProductsShopContext context)
        {
            var users = context.Users.Where(u => u.ProductsSold.Count(pr => pr.Buyer != null) != 0)
               .OrderBy(u => u.FirstName).ThenBy(u => u.LastName).
               Select(user => new
               {
                   user.FirstName,
                   user.LastName,
                   soldProducts = user.ProductsSold.Select(p => new
                   {
                       p.Name,
                       p.Price,
                       buyerFName=p.Buyer.FirstName,
                       buyerLName=p.Buyer.LastName
                   })
               });
            var usersAsJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("..//..//..//ExportedFiles//users-sold-products.json", usersAsJson);
        }

        private static void ExportProductsCategory(ProductsShopContext context)
        {

            var categoriesByProductsSoldCount = context.Categories
                           .OrderByDescending(c => c.Products.Count)
                           .Select(c => new
                           {
                               category = c.Name,
                               productsCount = c.Products.Count,
                               averagePrice = c.Products.Average(p => p.Price),
                               totalRevenue = (decimal?)c.Products.Sum(p => p.Price)
                           });

            // var categoryAsJson = JsonConvert.SerializeObject(categoryEntities, Formatting.Indented);
            ExportToJson(categoriesByProductsSoldCount, "..//..//..//ExportedFiles//categoriesByproducts.json");
        }

        private static void ExportToJson<TEntity>(TEntity categoryEntities, string path)
        {
            string jsonProducts = JsonConvert.SerializeObject(categoryEntities, Formatting.Indented);
            File.WriteAllText(path, jsonProducts);
        }

      

        private static void ExportProductsInRange(ProductsShopContext context)
        {
            var products = context.Products.Where(p => p.Price <= 1000 && p.Price >= 500 && p.Buyer == null).OrderBy(p => p.Price)
                .Select(p => new {
                    name=p.Name,
                    price=p.Price,
                    seller=p.Seller
                });
            var productsAsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText("..//..//..//ExportedFiles//products.json", productsAsJson);
        }


    }
}
