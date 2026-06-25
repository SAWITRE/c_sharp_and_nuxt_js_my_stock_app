using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStockApi.Models
{
    [Table("categories", Schema = "public")]
    public class Category
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}