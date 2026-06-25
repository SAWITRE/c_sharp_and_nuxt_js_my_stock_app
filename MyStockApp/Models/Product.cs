using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStockApi.Models
{ 
    [Table("products", Schema = "public")]
    public class Product
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("price")]
        public decimal Price { get; set; }

        [Column("stockquantity")]
        public int StockQuantity { get; set; }

        // Foreign Key
        [Column("categoryid")]
        public int CategoryId { get; set; }

        // Navigation Property (ช่วยให้ EF Core เชื่อมตารางให้เราอัตโนมัติ)
        // [ForeignKey("categoryid")]
        public Category? Category { get; set; }
    }
}