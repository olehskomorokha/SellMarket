using Microsoft.EntityFrameworkCore;
using SellMarket.Model.Entities;
namespace SellMarket.Model.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
             : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<UserAdress> UserAdresses { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductDetails> ProductDetails { get; set; }

    }
}
