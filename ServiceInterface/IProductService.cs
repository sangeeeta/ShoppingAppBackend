using ShoppingApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Interface
{
    public interface IProductService
    {

        Task<List<ProductModel>> GetProductsAsync();
        Task<ProductModel?> GetProductsByIdAsync(int id);
        Task SaveUpdateProductAsync(ProductModel productModel);
        Task DeleteProductAsync(int id);
    }
}
