namespace RepositoryPatternWithEFCore.EF4
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int? ReferenceId { get; set; }
        public int? StockTransactionId { get; set; }
        public StockTransaction? StockTransaction { get; set; }
    }

}
