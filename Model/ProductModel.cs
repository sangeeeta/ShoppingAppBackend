namespace ShoppingApp.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int size { get; set; }
        public decimal Price { get; set; }
    }
}
