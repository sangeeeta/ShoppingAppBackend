using Microsoft.EntityFrameworkCore;
using ShoppingApp.EfCore;
using System.Threading.Tasks;

namespace ShoppingApp.RepositoryInterface
{
    public interface IProductRepository
    {
        DbSet<Product> Products { get; }
        Task SaveChangesAsync();
    }
}
