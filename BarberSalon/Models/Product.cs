namespace BarberSalon.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string PhotoPath { get; set; } = "/assets/images/products/default-product.png";
    }
}
