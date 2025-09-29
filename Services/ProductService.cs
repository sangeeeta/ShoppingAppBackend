using ShoppingApp.Data;
using ShoppingApp.EfCore;
using ShoppingApp.Model;
using System.Xml.Linq;

namespace ShoppingApp.Services
{
    public class ProductService
    {
        private ShoppingDbContext _context;
        public ProductService(ShoppingDbContext context)
        {
            _context = context;
        }
        public List<ProductModel> GetProducts()
        {
            List<ProductModel> response = new List<ProductModel>();
            var dataList = _context.Products.ToList();
            dataList.ForEach(row => response.Add(new ProductModel()
            {
                Brand = row.Brand,
                Name = row.Name,
                Id=row.Id,
                Price = row.Price,
                size=row.size,
            }));
            return response;
        }

        public ProductModel GetProductsById(int id)
        {
            ProductModel response = new ProductModel();
            var dataList = _context.Products.Where(d => d.Id.Equals(id)).FirstOrDefault();
            return new ProductModel()
            {
                Brand = dataList.Brand,
                Name = dataList.Name,
                Id = dataList.Id,
                Price = dataList.Price,
                size = dataList.size
            };            
        }

        public void SaveUpdateProduct(ProductModel productModel)
        {
            if (productModel.Id > 0) // UPDATE
            {
                var dbTable = _context.Products.FirstOrDefault(d => d.Id == productModel.Id);
                if (dbTable != null)
                {
                    dbTable.Brand = productModel.Brand;
                    dbTable.Name = productModel.Name;
                    dbTable.size = productModel.size;
                    dbTable.Price = productModel.Price;
                    _context.Products.Update(dbTable);
                }
            }
            else // INSERT
            {
                var dbTable = new Product();
                {
                    dbTable.Brand = productModel.Brand;
                    dbTable.Name = productModel.Name;
                    dbTable.size = productModel.size;
                    dbTable.Price = productModel.Price;
                };
                dbTable.Id = 0; // optional, EF will auto-generate anyway
                _context.Products.Add(dbTable);
            }
            _context.SaveChanges();
        }

        public void DeleteProduct(int Id)
        {
            var product = _context.Products.FirstOrDefault(d => d.Id == Id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

        }
    }
}
