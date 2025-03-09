using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryPatternWithEFCore.EF4
{
    public class StockTransaction
    {
        [Key]
        public int Id { get; set; }
        public int StockId { get; set; }
        public int? DestinationWarehouseId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OldValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ChangeValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NewValue { get; set; }

        [EnumDataType(typeof(TransactionTypeEnum))]
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public int? ReferenceId { get; set; }
        public Stock ProductStock { get; set; } = null!;
        public Warehouse? DestinationWarehouse { get; set; }
        public virtual ICollection<AuditLog>? AuditLogs { get; set; }
    }
}
