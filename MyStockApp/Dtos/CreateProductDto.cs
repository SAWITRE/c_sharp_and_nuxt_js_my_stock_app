namespace MyStockApi.Dtos
{
    public class CreateProductDto
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

    }
}
