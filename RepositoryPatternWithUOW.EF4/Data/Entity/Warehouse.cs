namespace RepositoryPatternWithEFCore.EF4
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public ICollection<Stock> ProductStocks { get; set; } = new List<Stock>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
        public virtual ICollection<AuditLog>? AuditLogs { get; set; }
    }
}
