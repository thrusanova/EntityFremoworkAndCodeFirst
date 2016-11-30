
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

        }

        private static void ExportProductsInRange(ProductsShopContext context)
        {
            var products = context.Products.Where(p => p.Price <= 1000 && p.Price >= 500 && p.BuyerId == null).OrderBy(p => p.Price)
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
