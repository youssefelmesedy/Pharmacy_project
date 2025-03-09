using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryPatternWithEFCore.EF4
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }

}
