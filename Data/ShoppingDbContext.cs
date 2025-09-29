using Microsoft.EntityFrameworkCore;
using ShoppingApp.EfCore;

namespace ShoppingApp.Data
{
    public class ShoppingDbContext : DbContext
    {
        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductApproval> ProductApproval { get; set; }
    }
}
