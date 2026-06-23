using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStockApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Property (ช่วยให้ EF Core เชื่อมตารางให้เราอัตโนมัติ)
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}