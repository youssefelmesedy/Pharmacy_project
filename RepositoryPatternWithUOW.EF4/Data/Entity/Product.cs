using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryPatternWithEFCore.EF4
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }

        public DateTime DateAdded { get; private set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }

        public int? InternationalCode { get; set; }

        public string? ShortCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountRate { get; set; }

        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; } = null!;

        public ICollection<Stock> ProductStocks { get; set; } = new List<Stock>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
        public virtual ICollection<AuditLog>? AuditLogs { get; set; }
    }
}
