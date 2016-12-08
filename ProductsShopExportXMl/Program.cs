using ProductsShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductsShopExportXMl
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ProductsShopContext();
            ExportProductsInRange(context);
            ExportSoldProducts(context);
            ExportCategoriesByProductCount(context);
            ExportUsersAndProducts(context);
        }
        private static void ExportUsersAndProducts(ProductsShopContext context)
        {
            var users = context.Users.Where(user => user.ProductsSold.Count != 0)
                 .OrderByDescending(p => p.ProductsSold.Count).ThenBy(u => u.LastName)
                 .Select(user => new
                 {
                     FName = user.FirstName,
                     LName = user.LastName,
                     age = user.Age,
                     soldProducts = user.ProductsSold.Select(p => new
                     {
                       name=  p.Name,
                       price=  p.Price
                     })
                 });
            var xmlDocument = new XElement("users");
            xmlDocument.Add(new XAttribute("count", users.Count()));
            foreach (var user in users)
            {
                var userNode = new XElement("user");
                if (user.FName != null)
                {
                    userNode.Add(new XAttribute("first-name", user.FName));
                }
                if (user.LName != null)
                {
                    userNode.Add(new XAttribute("last-name", user.LName));
                }
                userNode.Add(new XAttribute("age", user.age));
                var xmlSoldProducts = new XElement("sold-products");
                foreach (var soldPr in user.soldProducts)
                {
                    var productNode = new XElement("product");
                    productNode.Add(new XAttribute("name", soldPr.name));
                    productNode.Add(new XAttribute("price", soldPr.price));
                    xmlSoldProducts.Add(productNode);
                }
                userNode.Add(xmlSoldProducts);
                xmlDocument.Add(userNode);
                xmlDocument.Save("..//..//..//ExportedFiles//users-and-products.xml");

            }

        }
        private static void ExportCategoriesByProductCount(ProductsShopContext context)
        {
            var categories = context.Categories.OrderBy(p => p.Products.Count)
                .Select(c => new
                {
                    name = c.Name,
                    products_count = c.Products.Count,
                    average_price = c.Products.Average(p => p.Price),
                    total_revenue = c.Products.Sum(p => p.Price)
                });
            var xmlDocument = new XElement("categories");
            foreach (var cat in categories)
            {
                var categoryNode = new XElement("category");
                categoryNode.Add(new XAttribute("name", cat.name));
                categoryNode.Add(new XElement("products-counts", cat.products_count));
                categoryNode.Add(new XElement("average-price", cat.average_price));
                categoryNode.Add(new XElement("total-revenue", cat.total_revenue));
                xmlDocument.Add(categoryNode);
            }
            xmlDocument.Save("..//..//..//ExportedFiles//categories-by-products.xml");

        }

        private static void ExportSoldProducts(ProductsShopContext context)
        {
            var products = context.Users
                 .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                 .OrderBy(u => u.FirstName)
                 .ThenBy(u => u.LastName).
                 Select(u => new
                 {
                     first_name = u.FirstName,
                     lasrt_name = u.LastName,
                     sold_products = u.ProductsSold.Select(p => new
                     {
                         name = p.Name,
                         price = p.Price,
                         buyer_first_name = p.Buyer.FirstName,
                         buyer_last_name = p.Buyer.LastName
                     })
                 });
            var xmlDocument = new XElement("users");
            foreach (var user in products)
            {
                var userNode = new XElement("user");
                if (user.first_name!=null)
                {
                    userNode.Add(new XAttribute("first-name",user.first_name));
                }
                if (user.lasrt_name != null) {
                    userNode.Add(new XElement("last-name", user.lasrt_name));
                }
                var xmlSoldProducts = new XElement("sold-products");
                foreach (var soldPr in user.sold_products)
                {
                    var productNode = new XElement("product");
                    productNode.Add(new XAttribute("name", soldPr.name));
                    productNode.Add(new XAttribute("price", soldPr.price));
                    if (soldPr.buyer_first_name!=null)
                    {
                        productNode.Add(new XAttribute("buyer-first-name", soldPr.buyer_first_name));
                    }
                    if (soldPr.buyer_last_name != null)
                    {
                        productNode.Add(new XAttribute("buyer-last-name", soldPr.buyer_last_name));
                    }
                    xmlSoldProducts.Add(productNode);
                }
                userNode.Add(xmlSoldProducts);
                xmlDocument.Add(userNode);
                xmlDocument.Save("..//..//..//ExportedFiles//users-sold-product.xml");
            }
        }
        private static void ExportProductsInRange(ProductsShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000 && p.Buyer == null)
                .Select(u => new
            {
               name = u.Name,
              price =  u.Price,
               seller =  u.Seller.FirstName+" "+u.Seller.LastName
            });
            var xmlProducts = new XElement("products");
            foreach (var pr in products)
            {
                var product = new XElement("product");
                product.Add(new XAttribute("name", pr.name));
                product.Add(new XAttribute("price", pr.price));
                product.Add(new XAttribute("seller", pr.seller));
                xmlProducts.Add(product);
            }
            xmlProducts.Save("..//..//..//ExportedFiles//users-and-products.xml");
        }
    }
}
