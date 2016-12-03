namespace ProductsShop.Data
{
    using Models;
    using System.Data.Entity;

    public class ProductsShopContext : DbContext
    {
    
        public ProductsShopContext()
            : base("name=ProductsShopContext")
        {
        }

        public virtual DbSet<User>Users { get; set; }
        public virtual DbSet<Product>Products { get; set; }
        public virtual DbSet<Category>Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
         
            modelBuilder.Entity<User>()
                .HasMany(user => user.Friends)
                .WithMany()
                .Map(u =>
                {
                    u.MapLeftKey("userId");
                    u.MapRightKey("friendId");
                    u.ToTable("UserFriends");
                });
            base.OnModelCreating(modelBuilder);
        }

    }

}