using ShoppingApp.Data;
using ShoppingApp.EfCore;
using ShoppingApp.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ShoppingApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingDbContext _context;

        public ProductRepository(ShoppingDbContext context)
        {
            _context = context;
        }

        public DbSet<Product> Products => _context.Products;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
