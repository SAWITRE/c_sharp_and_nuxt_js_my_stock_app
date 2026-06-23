namespace MyStockApi.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public int? CategoryId { get; set; } // หมวดหมู่ที่เกี่ยวข้อง (ถ้ามี)
        public string CategoryName { get; set; } = string.Empty; // ชื่อหมวดหมู่ที่ดึงมาจากตาราง Category
    }
}