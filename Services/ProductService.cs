using ShoppingApp.Interface;
using ShoppingApp.Model;
using ShoppingApp.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ShoppingApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            var products = await _productRepo.Products.ToListAsync();
            return products.Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Price = p.Price,
                size = p.size
            }).ToList();
        }

        public async Task<ProductModel?> GetProductsByIdAsync(int id)
        {
            var product = await _productRepo.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return null;

            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                size = product.size
            };
        }

        public async Task SaveUpdateProductAsync(ProductModel productModel)
        {
            if (productModel.Id > 0)
            {
                var dbProduct = await _productRepo.Products.FirstOrDefaultAsync(p => p.Id == productModel.Id);
                if (dbProduct != null)
                {
                    dbProduct.Name = productModel.Name;
                    dbProduct.Brand = productModel.Brand;
                    dbProduct.Price = productModel.Price;
                    dbProduct.size = productModel.size;
                    _productRepo.Products.Update(dbProduct);
                }
            }
            else
            {
                var dbProduct = new EfCore.Product
                {
                    Name = productModel.Name,
                    Brand = productModel.Brand,
                    Price = productModel.Price,
                    size = productModel.size
                };
                _productRepo.Products.Add(dbProduct);
            }

            await _productRepo.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var dbProduct = await _productRepo.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (dbProduct != null)
            {
                _productRepo.Products.Remove(dbProduct);
                await _productRepo.SaveChangesAsync();
            }
        }
    }
}
