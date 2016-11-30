namespace ProductsShop.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductsShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
      
    }
}
